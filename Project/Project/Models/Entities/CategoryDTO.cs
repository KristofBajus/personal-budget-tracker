using CommunityToolkit.Mvvm.ComponentModel;

namespace Project.Models.Entities
{
    public partial class CategoryDTO : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty]
        public partial string Name { get; set; }
        [ObservableProperty]
        public partial string Color { get; set; }
    }
}
