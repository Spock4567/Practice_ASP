namespace Practice_ASP
{
    public class RequestLimiterService
    {
        private int _limit;
        private int _requestsCount;

        public RequestLimiterService(int limit)
        {
            _limit = limit;
        }

        //пытаемся занять слот
        //текущее кол-во запросов увеличивается на 1
        //и сравнивается с лимитом
        //если больше, чем лимит, то слот недоступен,
        //а кол-во запросов уменьшается на 1
        public bool TryAcquireSlot()
        {
            if (Interlocked.Increment(ref _requestsCount) > _limit)
            {
                Interlocked.Decrement(ref _requestsCount);
                return false;
            }
            return true;
        }

        //освобождаем слот
        //текущее кол-во запросов уменьшается на 1
        public void ReleaseSlot()
        {
            Interlocked.Decrement(ref _requestsCount);
        }

       
    }
}
