using LMS_API.Models;
using System.Collections.Generic;

namespace LMS_API.Interfaces
{
    public interface IPublisherRepository
    {
        IEnumerable<Publisher> GetList(int publisherId = 0);
        string SavePublisher(Publisher publisher);
    }
}
