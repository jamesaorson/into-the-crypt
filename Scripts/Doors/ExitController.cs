namespace IntoTheCrypt.Doors
{
    public class ExitController : DoorController
    {
        #region Protected

        #region Member Methods
        protected override bool CanPlayerInteract()
        {
            return true;
        }

        protected override void Interact()
        {
            IsOpen = !IsOpen;
            //Door.SetActive(!IsOpen);
        }
        #endregion

        #endregion
    }
}