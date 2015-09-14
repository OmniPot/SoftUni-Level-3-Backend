namespace DistanceCalculator_REST.Controllers
{
    using System;
    using System.Web.Http;

    public class DistanceController : ApiController
    {
        [HttpGet]
        [Route("api/points/distance")]
        public IHttpActionResult CalculateDistance(int startX, int startY, int endX, int endY)
        {
            var deltaX = startX - endX;
            var deltaY = startY - endY;
            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            return this.Ok(distance);
        }
    }
}
