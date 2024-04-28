namespace blazam.org
{
    public delegate Task AppEvent();
    public delegate Task AppEvent<T>(T data);
}
