namespace IntoTheCrypt.Doors
{
    public class EntranceController : DoorController
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