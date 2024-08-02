using System.ComponentModel.DataAnnotations;

namespace DoAnWeb.Enums;

public enum EOrderStatus
{
    [Display(Name = "Hủy")]
    No0,

    [Display(Name = "Chờ xác nhận")]
    No1,
    
    [Display(Name = "Đã xác nhận")]
    No2,
    
    [Display(Name = "Đang vận chuyển")]
    No3,
    
    [Display(Name = "Đã nhận")]
    No4,
    
    [Display(Name = "WTF?????")]
    No5,

}
