namespace Api_Almacen.Models.DTOs
{
    public class SideMenu
    {
        public string? routeLink { get; set; }
        public string? icon { get; set; }
        public string? label { get; set; }
        public bool expanded { get; set; }
        public List<SideMenu>? items { get; set; }
    }
}
