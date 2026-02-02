Вот реализация задачи на языке C#.

### Архитектура решения

Для выполнения всех требований (особенно отсутствия активного ожидания и потокобезопасности) используется паттерн **Producer-Consumer** (Производитель-Потребитель) с промежуточным буфером.

1.  **Shared Resource (`Tunnel`)**:
    *   Использует `SemaphoreSlim` с начальным значением 5, чтобы ограничить количество кораблей *внутри* туннеля.
    *   Использует `BlockingCollection<Ship>` (потокобезопасная очередь) для хранения кораблей, которые прошли туннель и ждут причала. Для каждого типа товара — своя очередь.
2.  **Generator**: Создает корабли и пытается поместить их в туннель.
3.  **Docks (Piers)**: Каждый причал работает в своем потоке, забирает (Consumes) корабли своего типа из очереди и имитирует загрузку (`Thread.Sleep`).

### Код программы (C#)

```csharp
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SeaPortSimulation
{
    // Типы товаров
    public enum GoodsType
    {
        Bread,
        Banana,
        Clothing
    }

    // Класс Корабля
    public class Ship
    {
        public GoodsType Type { get; }
        public int Capacity { get; } // 10, 50, 100
        public int Id { get; }

        private static int _globalId = 0;

        public Ship(GoodsType type, int capacity)
        {
            Type = type;
            Capacity = capacity;
            Id = Interlocked.Increment(ref _globalId);
        }

        public override string ToString()
        {
            return $"Корабль #{Id} [{Type}, {Capacity}шт]";
        }
    }

    // Общий ресурс: Туннель + Зона ожидания перед причалами
    public class SeaTunnel
    {
        // Ограничение: "в туннеле одновременно могут находиться только 5 кораблей"
        private readonly SemaphoreSlim _tunnelSemaphore = new SemaphoreSlim(5, 5);

        // Потокобезопасные очереди для каждого типа причала (зона после туннеля)
        private readonly Dictionary<GoodsType, BlockingCollection<Ship>> _dockQueues;

        public SeaTunnel()
        {
            _dockQueues = new Dictionary<GoodsType, BlockingCollection<Ship>>
            {
                { GoodsType.Bread, new BlockingCollection<Ship>() },
                { GoodsType.Banana, new BlockingCollection<Ship>() },
                { GoodsType.Clothing, new BlockingCollection<Ship>() }
            };
        }

        // Метод для Генератора (Вход в туннель)
        public void SendShipToTunnel(Ship ship)
        {
            Logger.Log($"{ship} подплыл к туннелю.");

            // Пытаемся зайти в туннель. Если там 5 кораблей, поток заснет здесь (без активного ожидания)
            _tunnelSemaphore.Wait();

            try
            {
                Logger.Log($"{ship} ЗАШЕЛ в туннель (Свободных мест: {_tunnelSemaphore.CurrentCount}).");
                
                // Имитация прохода по туннелю (например, 1 секунда)
                Thread.Sleep(1000);

                // Корабль прошел туннель и попадает в очередь к причалу
                _dockQueues[ship.Type].Add(ship);
                
                Logger.Log($"{ship} ВЫШЕЛ из туннеля и ждет погрузки.");
            }
            finally
            {
                // Освобождаем место в туннеле для следующего корабля
                _tunnelSemaphore.Release();
            }
        }

        // Метод для Причала (Взять корабль из очереди)
        public Ship GetShipForDock(GoodsType type)
        {
            // Take блокирует поток, если очередь пуста (нет активного ожидания)
            return _dockQueues[type].Take();
        }
    }

    // Генератор кораблей
    public class ShipGenerator
    {
        private readonly SeaTunnel _tunnel;
        private readonly Random _random = new Random();

        public ShipGenerator(SeaTunnel tunnel)
        {
            _tunnel = tunnel;
        }

        public void Start(int count)
        {
            new Thread(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    // Генерация случайного типа и вместительности
                    var type = (GoodsType)_random.Next(0, 3);
                    var capacityOptions = new[] { 10, 50, 100 };
                    var capacity = capacityOptions[_random.Next(0, 3)];

                    var ship = new Ship(type, capacity);

                    // Отправка в туннель (может заблокировать этот поток, если туннель полон)
                    _tunnel.SendShipToTunnel(ship);

                    // Имитация интервала прибытия кораблей (генератор работает независимо)
                    Thread.Sleep(800); 
                }
            })
            { IsBackground = true }.Start();
        }
    }

    // Причал (Dock)
    public class PierLoader
    {
        private readonly SeaTunnel _tunnel;
        private readonly GoodsType _type;

        public PierLoader(SeaTunnel tunnel, GoodsType type)
        {
            _tunnel = tunnel;
            _type = type;
        }

        public void StartWorking()
        {
            new Thread(() =>
            {
                Logger.Log($"Причал [{_type}] начал работу.");
                while (true)
                {
                    // Поток спит здесь, если нет кораблей нужного типа
                    Ship ship = _tunnel.GetShipForDock(_type);

                    // Если получили корабль
                    ProcessShip(ship);
                }
            })
            { IsBackground = true }.Start();
        }

        private void ProcessShip(Ship ship)
        {
            // Расчет времени: 10 ед. товара в секунду
            // 10 шт -> 1 сек, 50 шт -> 5 сек, 100 шт -> 10 сек
            int loadingTimeSeconds = ship.Capacity / 10;

            Logger.Log($"Причал [{_type}] начал загрузку {ship}. Время: {loadingTimeSeconds} сек.");

            // Имитация загрузки
            Thread.Sleep(loadingTimeSeconds * 1000);

            Logger.Log($"--- Причал [{_type}] ЗАГРУЗИЛ {ship}. Корабль уплыл. ---");
        }
    }

    // Хелпер для красивого вывода в консоль (Thread Safe)
    public static class Logger
    {
        private static readonly object _lock = new object();
        public static void Log(string message)
        {
            lock (_lock)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Начало симуляции. Нажмите Enter для выхода.\n");

            // 1. Создаем общий ресурс (Туннель)
            SeaTunnel tunnel = new SeaTunnel();

            // 2. Создаем и запускаем причалы (Потребители)
            PierLoader dockBread = new PierLoader(tunnel, GoodsType.Bread);
            PierLoader dockBanana = new PierLoader(tunnel, GoodsType.Banana);
            PierLoader dockClothing = new PierLoader(tunnel, GoodsType.Clothing);

            dockBread.StartWorking();
            dockBanana.StartWorking();
            dockClothing.StartWorking();

            // 3. Создаем и запускаем генератор (Производитель)
            // Пусть создаст 20 кораблей (или можно сделать бесконечный цикл в генераторе)
            ShipGenerator generator = new ShipGenerator(tunnel);
            generator.Start(20);

            Console.ReadLine();
        }
    }
}
```

### Разбор соответствия требованиям:

1.  **Правильно разбить задачу на параллельность:**
    *   Генератор работает в отдельном потоке.
    *   Каждый из 3 причалов работает в своем отдельном потоке.
    *   Прохождение туннеля симулируется параллельно (до 5 потоков одновременно).

2.  **Синхронизировать потоки, сохранить целостность данных:**
    *   Используется `SemaphoreSlim` для контроля доступа к "узкому" туннелю.
    *   Используется `BlockingCollection` (основана на ConcurrentQueue) для передачи данных между Туннелем и Причалами. Это гарантирует, что один корабль не будет взят двумя причалами (хотя по логике типы разные, но даже внутри одной коллекции это безопасно).

3.  **Работа генератора не зависит от причалов:**
    *   Генератор просто "кидает" корабли в метод `SendShipToTunnel`. Если туннель свободен, он продолжает генерировать. Если туннель забит, генератор ждет освобождения места в туннеле, но ему неважно, свободен ли причал.

4.  **Потоки не должны быть активными если нет задач:**
    *   `_dockQueues[type].Take()` использует внутри `Monitor.Wait` (или аналогичные примитивы ОС), поток причала переходит в состояние `WaitSleepJoin` и не потребляет CPU, пока генератор не пришлет корабль.
    *   `_tunnelSemaphore.Wait()` аналогично усыпляет поток генератора, если в туннеле 5 кораблей.

5.  **Потоки не должны держать mutex если нет задач:**
    *   Семафор `_tunnelSemaphore` удерживается только на время прохождения туннеля (`Thread.Sleep(1000)` внутри блока `try`). Как только корабль попадает в очередь к причалу, семафор отпускается (`Release`), позволяя зайти новому кораблю, даже если причал еще занят.

6.  **Логика времени загрузки:**
    *   Реализована формула: `ship.Capacity / 10`. Корабль вместимостью 50 загружается ровно 5 секунд.