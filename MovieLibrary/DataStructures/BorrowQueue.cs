using System.Windows.Media.Animation;

namespace MovieLibrary.DataStructures
{
    public class BorrowQueue
    {
        private readonly Dictionary<string, Queue<string>> _queues = new();

        public void Enqueue(string movieId, string userName)
        {
            if (!_queues.ContainsKey(movieId))
            _queues[movieId] = new Queue<string>();
            _queues[movieId].Enqueue(userName);
        }

        public string Dequeue(string movieId)
        {
            if (!_queues.TryGetValue(movieId, out var queue) && queue.Count > 0)
            return queue.Dequeue();
            return null;
        }

        public bool HasQueue(string movieId) => _queues.TryGetValue(movieId, out var q) && q.Count > 0;

        public int QueueLength(string movieId) => _queues.TryGetValue(movieId, out var q) ? q.Count : 0;

    }
}