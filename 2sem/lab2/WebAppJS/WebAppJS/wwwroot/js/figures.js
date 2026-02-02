import * as THREE from 'three';

const divOutput = document.getElementById("Figures-output");

// ===== Сцена =====
const scene = new THREE.Scene();

// ===== Камера =====
const camera = new THREE.PerspectiveCamera(
    45,
    divOutput.clientWidth / divOutput.clientHeight,
    0.1,
    1000
);
camera.position.set(-30, 40, 30);
camera.lookAt(scene.position);

// ===== Рендерер =====
const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(divOutput.clientWidth, divOutput.clientHeight);
renderer.setClearColor(new THREE.Color(0xEEEEEE));
renderer.shadowMap.enabled = true;
renderer.shadowMap.type = THREE.PCFShadowMap;
divOutput.append(renderer.domElement);

// ===== Оси =====
scene.add(new THREE.AxesHelper(20));

// ===== Плоскость =====
const plane = new THREE.Mesh(
    new THREE.PlaneGeometry(60, 20),
    new THREE.MeshLambertMaterial({ color: 0xcccccc })
);
plane.rotation.x = -0.5 * Math.PI;
plane.receiveShadow = true;
scene.add(plane);

// ===== Свет =====
const spotLight = new THREE.SpotLight(0xffffff);
spotLight.position.set(-40, 60, -10);
spotLight.castShadow = true;
spotLight.shadow.mapSize.width = 2048;
spotLight.shadow.mapSize.height = 2048;
spotLight.intensity = 5000;
spotLight.target.position.set(0, 0, 0);
scene.add(spotLight);

// ===== Хранилище объектов =====
const meshes = new Map();

// ===== Функция добавления объекта =====
function addObject(obj) {
    let geometry;
    if (obj.type === "Box") geometry = new THREE.BoxGeometry(obj.a, obj.b, obj.c);
    if (obj.type === "Sphere") geometry = new THREE.SphereGeometry(obj.a, obj.b, obj.c);
    if (obj.type === "Tetrahedron") geometry = new THREE.TetrahedronGeometry(obj.a);

    const material = new THREE.MeshLambertMaterial({ color: Math.random() * 0xffffff });
    const mesh = new THREE.Mesh(geometry, material);

    mesh.castShadow = true;
    mesh.position.set(obj.x, obj.y, obj.z);
    mesh.userData.id = obj.id;

    scene.add(mesh);
    meshes.set(obj.id, mesh);

    // ===== Удаление по клику =====
    mesh.cursor = 'pointer'; // CSS указатель не работает, но логически
}

// ===== Удаление объекта =====
function removeObject(id) {
    const mesh = meshes.get(id);
    if (mesh) {
        scene.remove(mesh);
        meshes.delete(id);
    }
}

// ===== Загрузка объектов из БД =====
fetch('/api/scene')
    .then(res => res.json())
    .then(data => data.forEach(addObject));

// ===== Resize =====
window.addEventListener('resize', () => {
    const width = divOutput.clientWidth;
    const height = divOutput.clientHeight;
    camera.aspect = width / height;
    camera.updateProjectionMatrix();
    renderer.setSize(width, height);
});

// ===== Анимация =====
const raycaster = new THREE.Raycaster();
const mouse = new THREE.Vector2();

function animate() {
    requestAnimationFrame(animate);

    // Вращаем все объекты
    meshes.forEach(mesh => {
        mesh.rotation.x += 0.02;
        mesh.rotation.y += 0.02;
        mesh.rotation.z += 0.02;
    });

    renderer.render(scene, camera);
}
animate();

// ===== Обработка формы добавления =====
document.getElementById("addForm").addEventListener("submit", e => {
    e.preventDefault();
    const obj = {
        type: type.value,
        a: +a.value,
        b: +b.value,
        c: +c.value,
        x: +x.value,
        y: +y.value,
        z: +z.value
    };

    // Сохраняем на сервере
    fetch('/api/scene', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(obj)
    })
        .then(r => r.json())
        .then(savedObj => addObject(savedObj));

    a.value = b.value = c.value = x.value = y.value = z.value = '';
});

// ===== Удаление по клику на объект =====
divOutput.addEventListener('click', e => {
    // Преобразуем координаты клика в normalized device coords
    const rect = divOutput.getBoundingClientRect();
    mouse.x = ((e.clientX - rect.left) / rect.width) * 2 - 1;
    mouse.y = - ((e.clientY - rect.top) / rect.height) * 2 + 1;

    raycaster.setFromCamera(mouse, camera);
    const intersects = raycaster.intersectObjects([...meshes.values()]);
    if (intersects.length > 0) {
        const mesh = intersects[0].object;
        const id = mesh.userData.id;

        // Удаляем с сервера
        fetch(`/api/scene/${id}`, { method: 'DELETE' })
            .then(res => {
                if (res.ok) removeObject(id);
            });
    }
});
