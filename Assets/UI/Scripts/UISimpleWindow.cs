public abstract class UISimpleWindow<T> : UIWindow<T> where T : UISimpleWindow<T>
{
    public void Show()
    {
        Open();
    }

    public void Hide()
    {
        Close();
    }
	
}
