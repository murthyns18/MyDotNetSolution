using LMS_API.Models;
using System.Collections.Generic;

namespace LMS_API.Interfaces
{
    public interface IPublisherRepository
    {
        string DeletePublisher(int publisherID);

        /// <summary>
        /// To get the Publishers
        /// </summary>
        /// <param name="publisherId">-1, 0, id</param>
        /// <returns>Return the list of publishers</returns>
        IEnumerable<Publisher> GetList(int publisherId = 0);

        /// <summary>
        /// To Save the publisher
        /// </summary>
        /// <param name="publisher"></param>
        /// <returns>Return the msg publishers saved successfully</returns>
        string SavePublisher(Publisher publisher);
    }
}
