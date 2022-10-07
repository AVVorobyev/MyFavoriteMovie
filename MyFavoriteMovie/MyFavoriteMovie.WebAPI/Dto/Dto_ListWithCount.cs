namespace MyFavoriteMovie.WebAPI.Dto
{
    public class Dto_ListWithCount<T>
    {
        private readonly IEnumerable<T>? _list;
        private readonly int _count;

        public Dto_ListWithCount(IEnumerable<T>? list, int count)
        {
            _list = list;
            _count = count;
        }

        public IEnumerable<T>? List { get { return _list; } }
        public int Count { get { return _count; } }
    }
}
