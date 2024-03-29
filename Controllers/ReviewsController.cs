using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private TravelAPIContext _db = new TravelAPIContext();

        // GET api/reviews
        [HttpGet]
        public ActionResult<IEnumerable<Review>> Get()
        {
            return _db.Reviews
                // .Include(review => review.Destination)
                .ToList();
        }

        // POST api/reviews
        [HttpPost]
        public void Post([FromBody] Review review)
        {
            _db.Reviews.Add(review);
            var thisDestination = _db.Destinations
                .Include(destination => destination.Reviews)
                .FirstOrDefault(x => x.DestinationId == review.DestinationId);
            thisDestination.GetAvgRating();
            _db.SaveChanges();
        }

        // GET api/reviews/1
        [HttpGet("{id}")]
        public ActionResult<Review> Get(int id)
        {
            return _db.Reviews.FirstOrDefault(x => x.ReviewId == id);
        }

        // PUT api/reviews/1
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Review review)
        {
            review.ReviewId = id;
            _db.Entry(review).State = EntityState.Modified;
            _db.SaveChanges();
            var thisDestination = _db.Destinations
                .Include(destination => destination.Reviews)
                .FirstOrDefault(x=> x.DestinationId == review.DestinationId);
            thisDestination.GetAvgRating();
            _db.SaveChanges();
        }

        // DELETE api/reviews/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var thisReview = _db.Reviews.FirstOrDefault(x => x.ReviewId == id);
            _db.Reviews.Remove(thisReview);
            _db.SaveChanges();
            var thisDestination = _db.Destinations
                .Include(destination => destination.Reviews)
                .FirstOrDefault(x=> x.DestinationId == thisReview.DestinationId);
            thisDestination.GetAvgRating();
            _db.SaveChanges();
        }
    }
}