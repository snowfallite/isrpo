
//import * as THREE from 'three';
//import { OrbitControls } from 'OrbitControls';
//import { GLTFLoader } from 'GLTFLoader';

//// --- Контейнер ---
//const divOutput = document.getElementById("WebGL-output");

//// --- Сцена ---
//const scene = new THREE.Scene();
//scene.background = new THREE.Color(0xaaaaaa);

//// --- Камера ---
//const camera = new THREE.PerspectiveCamera(
//    45,
//    divOutput.clientWidth / divOutput.clientHeight,
//    0.1,
//    1000
//);
//camera.position.set(3, 2, 1); // ближе к автомобилю
//camera.lookAt(0, 0, 0);

//// --- Рендерер ---
//const renderer = new THREE.WebGLRenderer({ antialias: true });
//renderer.setSize(divOutput.clientWidth, divOutput.clientHeight);
//renderer.shadowMap.enabled = true;
//renderer.shadowMap.type = THREE.PCFSoftShadowMap; // мягкие тени
//divOutput.append(renderer.domElement);

//// --- OrbitControls ---
//const controls = new OrbitControls(camera, renderer.domElement);
//controls.enableDamping = true;
//controls.dampingFactor = 0.05;

//// --- Свет ---
//// Полусферический свет для мягкого окружающего освещения
//const hemiLight = new THREE.HemisphereLight(0xffffff, 0x444444, 0.5);
//hemiLight.position.set(0, 20, 0);
//scene.add(hemiLight);

//// Направленный свет для теней
//const dirLight = new THREE.DirectionalLight(0xffffff, 1);
//dirLight.position.set(5, 10, 5);
//dirLight.castShadow = true;

//// Настройка камеры теней для лучшего качества
//dirLight.shadow.mapSize.width = 2048;
//dirLight.shadow.mapSize.height = 2048;
//dirLight.shadow.camera.near = 0.5;
//dirLight.shadow.camera.far = 50;
//dirLight.shadow.camera.left = -10;
//dirLight.shadow.camera.right = 10;
//dirLight.shadow.camera.top = 10;
//dirLight.shadow.camera.bottom = -10;

//scene.add(dirLight);

//// --- Плоскость для теней ---
//const planeGeometry = new THREE.PlaneGeometry(50, 50);
//const planeMaterial = new THREE.ShadowMaterial({ opacity: 0.3 });
//const plane = new THREE.Mesh(planeGeometry, planeMaterial);
//plane.rotation.x = -Math.PI / 2; // горизонтально
//plane.position.y = 0;
//plane.receiveShadow = true;
//scene.add(plane);

//// --- Загрузка модели glTF ---
//const loader = new GLTFLoader();
//loader.load(
//    '/models/porsche959.glb',
//    (gltf) => {
//        const model = gltf.scene;

//        // Настройка всех мешей для теней
//        model.traverse((child) => {
//            if (child.isMesh) {
//                child.castShadow = true;
//                child.receiveShadow = true;
//                child.material.side = THREE.DoubleSide;
//            }
//        });

//        scene.add(model);
//        console.log("3D model loaded successfully");
//    },
//    undefined,
//    (error) => {
//        console.error("Error loading 3D model:", error);
//    }
//);

//// --- Обработка ресайза окна ---
//window.addEventListener('resize', () => {
//    const width = divOutput.clientWidth;
//    const height = divOutput.clientHeight;
//    camera.aspect = width / height;
//    camera.updateProjectionMatrix();
//    renderer.setSize(width, height);
//});

//// --- Анимация ---
//function renderScene() {
//    controls.update();
//    renderer.render(scene, camera);
//}

//renderer.setAnimationLoop(renderScene);




//-------------------------------------------------------------------------------

import * as THREE from 'three';
import { OrbitControls } from 'OrbitControls';
import { STLLoader } from 'STLLoader'; // Импортируем STLLoader

// Получаем контейнер
let divOutput = document.getElementById("WebGL-output");

// Сцена
let scene = new THREE.Scene();
scene.background = new THREE.Color(0xaaaaaa); // Сделал фон чуть темнее, чтобы серую модель было видно

// Камера
let camera = new THREE.PerspectiveCamera(
    45,
    divOutput.clientWidth / divOutput.clientHeight,
    0.1,
    1000
);

// Рендерер
let renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(divOutput.clientWidth, divOutput.clientHeight);
renderer.shadowMap.enabled = true;
renderer.shadowMap.type = THREE.PCFShadowMap;

divOutput.append(renderer.domElement);

// Управление камерой
const controls = new OrbitControls(camera, renderer.domElement);
controls.enableDamping = true;

// Позиция камеры (как вы просили)
camera.position.set(3, 2, 3); 
camera.lookAt(0, 0, 0);
controls.update();

// --- СВЕТ ---
// Добавим полусферический свет для красивых бликов
const hemiLight = new THREE.HemisphereLight(0xffffff, 0x444444);
hemiLight.position.set(0, 20, 0);
scene.add(hemiLight);

// Направленный свет (создает тени)
const dirLight = new THREE.DirectionalLight(0xffffff);
dirLight.position.set(3, 10, 10);
dirLight.castShadow = true;
scene.add(dirLight);

// --- ЗАГРУЗКА STL МОДЕЛИ ---
const loader = new STLLoader();

loader.load('/models/smoki.stl',
    function (geometry) {
        
        const material = new THREE.MeshPhongMaterial({
            color: 0xff5533, // Оранжевый цвет модели
            specular: 0x111111,
            shininess: 200
        });

        
        // 2. Создаем Меш (Объект = Геометрия + Материал)
        const mesh = new THREE.Mesh(geometry, material);

        // 3. Настройки объекта
        mesh.castShadow = true;
        mesh.receiveShadow = true;

        // Полезно: центрируем геометрию, чтобы модель вращалась вокруг своего центра
        geometry.center();


        
        mesh.rotation.x = Math.PI;
        scene.add(mesh);

        console.log("STL Model loaded");
    },
    (xhr) => {
        // Прогресс загрузки
        console.log((xhr.loaded / xhr.total * 100) + '% loaded');
    },
    (error) => {
        console.error('An error happened:', error);
    }
);

// --- RESIZE ---
window.addEventListener('resize', () => {
    const width = divOutput.clientWidth;
    const height = divOutput.clientHeight;
    camera.aspect = width / height;
    camera.updateProjectionMatrix();
    renderer.setSize(width, height);
});

// --- АНИМАЦИЯ ---
function renderScene() {
    controls.update();
    renderer.render(scene, camera);
}

renderer.setAnimationLoop(renderScene);