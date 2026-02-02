import * as THREE from 'three';
import { GLTFLoader } from 'GLTFLoader';
// import { OrbitControls } from 'OrbitControls'; // 1. Убираем OrbitControls, он нам больше не нужен для слежения

let scene, camera, renderer; // Убрали controls из переменных
let model, mixer, clips, clock;
let obstacles = [];

let isRunning = false;
let speed = 0.08;
let direction = 0;

// Настройки камеры (дистанция от робота)
const cameraParams = {
    offset: new THREE.Vector3(0, 5, -10), // Позиция камеры относительно робота (X, Y, Z)
    lookAtOffset: new THREE.Vector3(0, 2, 0) // Точка, куда смотрит камера (чуть выше ног)
};

init();
animate();

/* ---------------- INIT ---------------- */

function init() {

    scene = new THREE.Scene();
    scene.background = new THREE.Color(0xaaaaaa);

    camera = new THREE.PerspectiveCamera(
        45,
        window.innerWidth / window.innerHeight,
        0.1,
        1000
    );

    // Начальная позиция (изменится при первом кадре анимации)
    camera.position.set(0, 5, -10);

    renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setSize(window.innerWidth, window.innerHeight);
    document.body.append(renderer.domElement);

    // controls = new OrbitControls(camera, renderer.domElement); // 2. Отключаем создание контролов
    // controls.target.set(0, 2, 0);
    // controls.enableDamping = true;

    clock = new THREE.Clock();

    /* ---------- LIGHT ---------- */
    const hemiLight = new THREE.HemisphereLight(0xffffff, 0x444444, 4);
    hemiLight.position.set(0, 20, 0);
    scene.add(hemiLight);

    const dirLight = new THREE.DirectionalLight(0xffffff);
    dirLight.position.set(0, 20, 10);
    scene.add(dirLight);

    /* ---------- GRID ---------- */
    const grid = new THREE.GridHelper(200, 40, 0x000000, 0x000000);
    grid.material.opacity = 0.2;
    grid.material.transparent = true;
    scene.add(grid);

    /* ---------- OBSTACLES ---------- */
    const boxGeo = new THREE.BoxGeometry(2, 2, 2);
    const boxMat = new THREE.MeshStandardMaterial({ color: 0x333333 });

    for (let i = 0; i < 5; i++) {
        const box = new THREE.Mesh(boxGeo, boxMat);
        box.position.set(Math.random() * 20 - 10, 1, Math.random() * 20 - 10);
        scene.add(box);
        obstacles.push(box);
    }

    /* ---------- MODEL ---------- */
    const loader = new GLTFLoader();
    loader.load(
        '/models/RobotExpressive.glb',
        gltf => {
            model = gltf.scene;
            scene.add(model);

            mixer = new THREE.AnimationMixer(model);
            clips = gltf.animations;

            playAnimation('Idle');
        }
    );

    /* ---------- EVENTS ---------- */
    window.addEventListener('resize', onResize);
    document.addEventListener('keydown', onKeyDown);
    document.addEventListener('keyup', onKeyUp);
}

/* ---------------- ANIMATION ---------------- */

function playAnimation(name, loop = THREE.LoopRepeat) {
    if (!mixer) return;
    const clip = THREE.AnimationClip.findByName(clips, name);
    const action = mixer.clipAction(clip);
    mixer.stopAllAction();
    action.reset();
    action.loop = loop;
    action.play();
}

/* ---------------- KEYBOARD ---------------- */

function onKeyDown(e) {
    if (!model) return;

    switch (e.code) {
        case 'KeyW':
            if (!isRunning) {
                isRunning = true;
                playAnimation('Running');
            }
            break;
        case 'KeyA':
            model.rotation.y += 0.1;
            direction = model.rotation.y;
            break;
        case 'KeyD':
            model.rotation.y -= 0.1;
            direction = model.rotation.y;
            break;
        case 'Space':
            playAnimation('Jump', THREE.LoopOnce);
            break;
        case 'KeyC':
            playAnimation('Dance');
            break;
        case 'KeyV':
            playAnimation('Walking');
            break;
        case 'KeyI':
            playAnimation('Idle');
            break;
    }
}

function onKeyUp(e) {
    if (e.code === 'KeyW') {
        isRunning = false;
        playAnimation('Idle');
    }
}

/* ---------------- COLLISION ---------------- */

function checkCollision(nextPos) {
    const origin = model.position.clone();
    origin.y += 1;
    const direction = nextPos.clone().sub(model.position).normalize();
    const raycaster = new THREE.Raycaster(origin, direction, 0, 0.8);
    const intersects = raycaster.intersectObjects(obstacles, false);
    return intersects.length > 0;
}

/* ---------------- LOOP ---------------- */

function animate() {
    const dt = clock.getDelta();

    if (mixer) mixer.update(dt);

    if (model) {
        // --- Логика движения робота ---
        if (isRunning) {
            const angle = model.rotation.y;
            const dx = Math.sin(angle) * speed;
            const dz = Math.cos(angle) * speed;

            const nextPos = model.position.clone();
            nextPos.x += dx;
            nextPos.z += dz;

            if (!checkCollision(nextPos)) {
                model.position.copy(nextPos);
            }
        }

        // --- ЛОГИКА СЛЕЖЕНИЯ КАМЕРЫ ---

        // 1. Вычисляем смещение камеры с учетом поворота робота
        // Создаем копию вектора смещения, чтобы не менять оригинал
        const relativeCameraOffset = cameraParams.offset.clone();

        // Поворачиваем это смещение на угол поворота робота (вокруг оси Y)
        relativeCameraOffset.applyAxisAngle(new THREE.Vector3(0, 1, 0), model.rotation.y);

        // 2. Вычисляем целевую позицию камеры (Позиция робота + повернутое смещение)
        const targetCameraPosition = model.position.clone().add(relativeCameraOffset);

        // 3. Плавно перемещаем камеру (Lerp)
        // 0.05 - коэффициент плавности (чем меньше, тем медленнее камера догоняет)
        camera.position.lerp(targetCameraPosition, 0.1);

        // 4. Камера всегда смотрит на робота (с небольшим смещением вверх)
        const lookAtPosition = model.position.clone().add(cameraParams.lookAtOffset);
        camera.lookAt(lookAtPosition);
    }

    // controls.update(); // Отключено
    renderer.render(scene, camera);
    requestAnimationFrame(animate);
}

/* ---------------- RESIZE ---------------- */

function onResize() {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
}