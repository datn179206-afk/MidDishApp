namespace MidDishApp.Services;

public interface ILocalizer
{
    /// <summary>Current language: "en" (default) or "vi".</summary>
    string Lang { get; }
    string this[string key] { get; }
}

public class Localizer : ILocalizer
{
    private readonly IHttpContextAccessor _accessor;
    public Localizer(IHttpContextAccessor accessor) => _accessor = accessor;

    public string Lang =>
        _accessor.HttpContext?.Request.Cookies["lang"] == "vi" ? "vi" : "en";

    public string this[string key]
    {
        get
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            if (!Translations.Map.TryGetValue(key, out var t)) return key; // unknown key / framework message
            return Lang == "vi" ? t.Vi : t.En;
        }
    }
}

public static class Translations
{
    public static readonly Dictionary<string, (string En, string Vi)> Map = new()
    {
        // Layout / menu
        ["App.Title"] = ("Dish Management", "Quản lý món ăn"),
        ["Menu.Dishes"] = ("Dishes", "Món ăn"),
        ["Menu.Categories"] = ("Dish categories", "Loại món ăn"),

        // Dish list + filters
        ["Dish.List"] = ("Dish list", "Danh sách món ăn"),
        ["Btn.Create"] = ("Create new", "Thêm mới"),
        ["Filter.Search"] = ("Search by name", "Tìm theo tên"),
        ["Filter.SearchPlaceholder"] = ("Part of the dish name", "Một phần tên món"),
        ["Filter.AllCategories"] = ("All categories", "Tất cả loại món"),
        ["Filter.Status"] = ("Status", "Trạng thái"),
        ["Filter.AllStatus"] = ("All", "Tất cả"),
        ["Filter.MinPrice"] = ("Price from", "Giá từ"),
        ["Filter.MaxPrice"] = ("Price to", "Giá đến"),
        ["Filter.Sort"] = ("Sort by", "Sắp xếp"),
        ["Sort.Default"] = ("Default", "Mặc định"),
        ["Sort.PriceAsc"] = ("Price: low to high", "Giá tăng dần"),
        ["Sort.PriceDesc"] = ("Price: high to low", "Giá giảm dần"),
        ["Sort.TimeAsc"] = ("Preparation time: shortest first", "Thời gian chế biến tăng dần"),
        ["Btn.Search"] = ("Search", "Tìm kiếm"),
        ["Btn.Reset"] = ("Reset", "Đặt lại"),
        ["Col.Image"] = ("Image", "Ảnh"),
        ["Col.Actions"] = ("Actions", "Thao tác"),
        ["Dish.Name"] = ("Name", "Tên món"),
        ["Dish.Category"] = ("Category", "Loại món"),
        ["Dish.Price"] = ("Price", "Giá"),
        ["Dish.PrepTime"] = ("Preparation time", "Thời gian chế biến"),
        ["Dish.Description"] = ("Description", "Mô tả"),
        ["Dish.IsAvailable"] = ("Available for sale", "Đang bán"),
        ["Dish.Status"] = ("Status", "Trạng thái"),
        ["Status.Available"] = ("Available", "Đang bán"),
        ["Status.Unavailable"] = ("Unavailable", "Ngừng bán"),
        ["Time.Quick"] = ("Quick", "Nhanh"),
        ["Time.Regular"] = ("Regular", "Thông thường"),
        ["Unit.Minutes"] = ("min", "phút"),
        ["Btn.Detail"] = ("Details", "Chi tiết"),
        ["Btn.Edit"] = ("Edit", "Sửa"),
        ["Btn.Delete"] = ("Delete", "Xóa"),
        ["Btn.Save"] = ("Save", "Lưu"),
        ["Btn.Back"] = ("Back to list", "Quay lại danh sách"),
        ["Img.None"] = ("No image", "Chưa có ảnh"),
        ["List.NoResults"] = ("No dishes match your search.", "Không có món ăn nào phù hợp."),
        ["Confirm.Delete"] = ("Are you sure you want to delete this item?", "Bạn có chắc chắn muốn xóa mục này?"),

        // Create / Edit / Detail
        ["Dish.Create"] = ("Create dish", "Thêm món ăn"),
        ["Dish.Edit"] = ("Edit dish", "Sửa món ăn"),
        ["Dish.Detail"] = ("Dish details", "Chi tiết món ăn"),
        ["Form.SelectCategory"] = ("-- Select category --", "-- Chọn loại món --"),
        ["Img.Gallery"] = ("Images", "Hình ảnh"),
        ["Img.Thumbnail"] = ("Thumbnail", "Ảnh đại diện"),
        ["Img.SetThumbnail"] = ("Set as thumbnail", "Đặt làm ảnh đại diện"),
        ["Img.Add"] = ("Add image", "Thêm ảnh"),
        ["Img.Url"] = ("Image URL", "Đường dẫn ảnh"),
        ["Img.MakeThumb"] = ("Use as thumbnail", "Đặt làm ảnh đại diện"),
        ["Img.Delete"] = ("Delete image", "Xóa ảnh"),
        ["Img.Empty"] = ("This dish has no images yet.", "Món này chưa có ảnh nào."),

        // Categories
        ["Cat.List"] = ("Dish categories", "Loại món ăn"),
        ["Cat.Create"] = ("Create category", "Thêm loại món"),
        ["Cat.Edit"] = ("Edit category", "Sửa loại món"),
        ["Cat.Name"] = ("Category name", "Tên loại món"),
        ["Cat.Description"] = ("Description", "Mô tả"),
        ["Cat.DishCount"] = ("Dishes", "Số món"),

        // Messages
        ["Msg.Saved"] = ("Saved successfully.", "Lưu thành công."),
        ["Msg.Deleted"] = ("Deleted successfully.", "Đã xóa thành công."),
        ["Msg.DeleteFailed"] = ("Delete failed.", "Xóa thất bại."),
        ["Msg.CategoryInUse"] = ("Cannot delete this category because dishes are using it.", "Không thể xóa loại món đang có món ăn sử dụng."),
        ["Msg.ImageAdded"] = ("Image added.", "Đã thêm ảnh."),
        ["Msg.ThumbSet"] = ("Thumbnail updated.", "Đã đổi ảnh đại diện."),
        ["Msg.ImageDeleted"] = ("Image deleted.", "Đã xóa ảnh."),

        // Errors
        ["Err.DishNotFound"] = ("Dish not found.", "Không tìm thấy món ăn."),
        ["Err.CategoryNotFound"] = ("Category not found.", "Không tìm thấy loại món ăn."),
        ["Err.ImageNotFound"] = ("Image not found.", "Không tìm thấy ảnh."),
        ["Err.PageNotFound"] = ("Page not found.", "Không tìm thấy trang."),
        ["Err.Generic"] = ("Something went wrong.", "Đã xảy ra lỗi."),
        ["Err.PriceRange"] = ("Invalid price range: prices must be 0 or more and 'from' must not exceed 'to'.", "Khoảng giá không hợp lệ: giá phải từ 0 trở lên và 'từ' không được lớn hơn 'đến'."),
        ["Err.PriceInvalid"] = ("The price filter must be a valid number.", "Giá dùng để lọc phải là số hợp lệ."),

        // Validation
        ["Val.NameRequired"] = ("Name is required.", "Tên không được để trống."),
        ["Val.NameLength"] = ("Name must be at most 100 characters.", "Tên tối đa 100 ký tự."),
        ["Val.NameDuplicate"] = ("A dish with this name already exists in this category.", "Tên món ăn đã tồn tại trong loại món này."),
        ["Val.PriceRange"] = ("Price must be greater than 0.", "Giá phải lớn hơn 0."),
        ["Val.TimeRange"] = ("Preparation time must be greater than 0.", "Thời gian chế biến phải lớn hơn 0."),
        ["Val.CategoryRequired"] = ("Please select a category.", "Vui lòng chọn loại món."),
        ["Val.CategoryNotFound"] = ("The selected category does not exist.", "Loại món đã chọn không tồn tại."),
        ["Val.DescLength"] = ("Description is too long (max 500 characters).", "Mô tả quá dài (tối đa 500 ký tự)."),
        ["Val.CatNameRequired"] = ("Category name is required.", "Tên loại món không được để trống."),
        ["Val.CatNameDuplicate"] = ("This category name already exists.", "Tên loại món đã tồn tại."),
        ["Val.Number"] = ("Please enter a valid number.", "Vui lòng nhập số hợp lệ."),
        ["Val.Invalid"] = ("The value is invalid.", "Giá trị không hợp lệ."),
        ["Val.Required"] = ("This field is required.", "Trường này là bắt buộc."),
        ["Val.UrlRequired"] = ("Image URL is required.", "Đường dẫn ảnh không được để trống."),
        ["Val.UrlInvalid"] = ("Image URL must be a valid http/https address (max 500 characters).", "Đường dẫn ảnh phải là địa chỉ http/https hợp lệ (tối đa 500 ký tự)."),
    };
}
