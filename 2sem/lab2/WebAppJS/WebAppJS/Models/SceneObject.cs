namespace WebAppJS.Models
{
    public class SceneObject
    {
        public int Id { get; set; }

        public string Type { get; set; } = null!;
        // Box | Sphere | Tetrahedron

        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

}
