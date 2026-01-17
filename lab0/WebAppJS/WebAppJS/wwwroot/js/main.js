import { Point } from './Point.js';
import * as THREE from 'three';
//alert("Hello world")

//let x = 3
//let y = 5

//console.log("RES : " + x + y)
////-------------------
//console.log(typeof "стрка")
//console.log(typeof 42)
//console.log(typeof true)
//console.log(typeof null)
//console.log(typeof undefined)
//console.log(typeof Symbol())
//console.log(typeof 10n)

////-------------------
//let user = {
//    name: "Artyom",
//    surname: "Osipov",
//    age: 19
//}

//console.log(user.name + " " + user.surname)


////-------------------

//class Rectangle {
//    constructor(left, top, width, height) {
//        this.left = left;
//        this.top = top;
//        this.width = width;
//        this.height = height;
//    }

//    get area() {
//        return this.width * this.height;
//    }

//    move(dx, dy) {
//        this.left += dx;
//        this.top += dy;
//    }

//    overlap(rect) {
//        const l1 = new Point(this.left, this.top)
//        const r1 = new Point(this.left + this.width, this.top - this.height)

//        const l2 = new Point(rect.left, rect.top)
//        const r2 = new Point(rect.left + rect.width, rect.top - rect.height)

//        // один из прямоугольников справа от другого
//        if (l1.x >= r2.x || l2.x >= r1.x) { return false; }

//        // один из прямоугольников ниже другого
//        if (l1.y <= r2.y || l2.y <= r1.y) { return false; }

//        return true;
//    }


//}




//const rect1 = new Rectangle(10, 10, 100, 50)
//const rect2 = new Rectangle(50, 20, 200, 80)

//console.log(rect1)
//console.log(rect2)

//console.log(rect1.area)
//console.log(rect2.area)

//console.log(rect1)

//console.log(rect1.overlap(rect2))
//rect1.move(10, 90)
//console.log(rect1.overlap(rect2))

////-------------------------------------------------------

//class game {
//    constructor(name) {
//        this.days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
//        this.name = name;
//        this.mouse = { x: 0, y: 0 };

//        document.addEventListener('mousemove', this.mouseMove.bind(this));
//    }

//    mouseMove(evt) {
//        console.log(this.name);
//        this.x = evt.clientX;
//        this.y = evt.clientY;
//    }

//    showDays() {
//        this.days.forEach(day => {
//            console.log(`global name = '${name}' game name = '${this.name}'  day = '${day}'`);
//        });
//    }

//}

//const myGame = new game('Scope example');
//myGame.showDays()



let divOutput = document.getElementById("WebGL-output");

// Сцена
let scene = new THREE.Scene();

// Камера
let camera = new THREE.PerspectiveCamera(
    45,
    divOutput.clientWidth / divOutput.clientHeight,
    0.1,
    1000
);

// Рендерер
let renderer = new THREE.WebGLRenderer();
renderer.setClearColor(new THREE.Color(0xEEEEEE));
renderer.setSize(divOutput.clientWidth, divOutput.clientHeight);
renderer.shadowMap.enabled = true;
renderer.shadowMap.type = THREE.PCFShadowMap;

// Оси
scene.add(new THREE.AxesHelper(20));

// Плоскость
let plane = new THREE.Mesh(
    new THREE.PlaneGeometry(60, 20),
    new THREE.MeshLambertMaterial({ color: 0xcccccc })
);
plane.rotation.x = -0.5 * Math.PI;
plane.receiveShadow = true;
scene.add(plane);

// Куб
let cube = new THREE.Mesh(
    new THREE.BoxGeometry(4, 4, 4),
    new THREE.MeshLambertMaterial({ color: 0xff0000 })
);
cube.position.set(-4, 3, 0);
cube.castShadow = true;
scene.add(cube);

// Сфера
let sphere = new THREE.Mesh(
    new THREE.SphereGeometry(4, 20, 20),
    new THREE.MeshLambertMaterial({ color: 0x7777ff })
);
sphere.position.set(20, 4, 2);
sphere.castShadow = true;
scene.add(sphere);

// Камера
camera.position.set(-30, 40, 30);
camera.lookAt(scene.position);

// Свет
let spotLight = new THREE.SpotLight(0xffffff);
spotLight.position.set(-40, 60, -10);
spotLight.castShadow = true;
spotLight.shadow.mapSize.width = 2048;
spotLight.shadow.mapSize.height = 2048;
spotLight.intensity = 5000;
scene.add(spotLight);

// Вставка canvas
divOutput.append(renderer.domElement);

// Resize
window.addEventListener('resize', () => {
    const width = divOutput.clientWidth;
    const height = divOutput.clientHeight;
    camera.aspect = width / height;
    camera.updateProjectionMatrix();
    renderer.setSize(width, height);
});

// Анимация
let step = 0;
function renderScene() {
    cube.rotation.x += 0.02;
    cube.rotation.y += 0.02;
    cube.rotation.z += 0.02;

    step += 0.04;
    sphere.position.x = 20 + 10 * Math.cos(step);
    sphere.position.y = 2 + 10 * Math.abs(Math.sin(step));

    renderer.render(scene, camera);
}

renderer.setAnimationLoop(renderScene);
