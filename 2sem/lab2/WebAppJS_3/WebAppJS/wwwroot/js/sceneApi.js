import * as THREE from 'three';
import { scene } from './main.js';

const meshes = new Map();

export function addObject(obj) {
    let geometry;
    if (obj.type === "Box") geometry = new THREE.BoxGeometry(obj.a, obj.b, obj.c);
    if (obj.type === "Sphere") geometry = new THREE.SphereGeometry(obj.a, obj.b, obj.c);
    if (obj.type === "Tetrahedron") geometry = new THREE.TetrahedronGeometry(obj.a);

    const material = new THREE.MeshNormalMaterial();
    const mesh = new THREE.Mesh(geometry, material);
    mesh.position.set(obj.x, obj.y, obj.z);
    mesh.userData.id = obj.id;

    scene.add(mesh);
    meshes.set(obj.id, mesh);
}

export function removeObject(id) {
    const mesh = meshes.get(id);
    if (mesh) {
        scene.remove(mesh);
        meshes.delete(id);
    }
}
