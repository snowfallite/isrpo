namespace WebAppJS.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    

    [ApiController]
    [Route("api/scene")]
    public class SceneObjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SceneObjectsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.SceneObjects.ToList());
        }

        [HttpPost]
        public IActionResult Create(Models.SceneObject obj)
        {
            _db.SceneObjects.Add(obj);
            _db.SaveChanges();
            return Ok(obj);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var obj = _db.SceneObjects.Find(id);
            if (obj == null) return NotFound();

            _db.SceneObjects.Remove(obj);
            _db.SaveChanges();
            return Ok();
        }
    }

}
