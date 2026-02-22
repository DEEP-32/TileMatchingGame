
namespace UIBase.UI{
    public abstract class BaseGameHUD : UiScreen{
        public override void Initialize(){
            CacheComponents();
            gameObject.SetActive(true);
        }
    }
}