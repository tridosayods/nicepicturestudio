using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace NicePictureStudio.Utils
{
    public static class Constant
    {
        public static readonly string UNDEFINED = "undefined";
        public static readonly int DEFAULT = 0;
        public static readonly string TITLE_MR = "นาย";
        public static readonly string TITLE_MS = "นางสาว";
        public static readonly string TITLE_MRS = "นาง";

        public static readonly string PHONE_NUMBER_DEFAULT = "กรุณาใส่ข้อมูลเพิ่มเติม";

        public static readonly int BOOKING_STATUS_NEW = 1;
        public static readonly int BOOKING_STATUS_CONFIRM = 2;
        public static readonly int BOOKING_STATUS_OPERATED = 3;
        public static readonly int BOOKING_STATUS_CANCEL = 4;
        
        public static readonly int SERVICE_STATUS_NEW = 1;
        public static readonly int SERVICE_STATUS_CONFIRM = 2;
        public static readonly int SERVICE_STATUS_COMPLETE = 3;
        public static readonly int SERVICE_STATUS_CANCEL = 4;
        public static readonly int SERVICE_STATUS_CANCEL_IN7DAYS = 5;

        public static readonly int SERVICE_TYPE_PREWEDDING = 1;
        public static readonly int SERVICE_TYPE_ENGAGEMENT = 2;
        public static readonly int SERVICE_TYPE_WEDDING = 3;

        public static readonly string SERVICE_TYPE_PREWEDDING_NAME = "PreWedding";
        public static readonly string SERVICE_TYPE_ENGAGEMENT_NAME = "Engagement";
        public static readonly string SERVICE_TYPE_WEDDING_NAME = "Wedding";

        public static readonly int PROMORION_ACTIVE = 1;
        public static readonly int PROMORION_DEACTIVE = 2;

        public static readonly int OUTSOURCE_STATUS_VACANT = 1;
        public static readonly int OUTSOURCE_STATUS_NOTVACANT = 2;

        public static readonly int OUTSOURCE_SERVICE_TYPE_PHOTOGRAPH = 1;
        public static readonly int OUTSOURCE_SERVICE_TYPE_CAMERAMAN = 2;
        public static readonly int OUTSOURCE_SERVICE_TYPE_COSMETIC = 3;
        public static readonly int OUTSOURCE_SERVICE_TYPE_WEDDINGSUITE = 4;
        public static readonly int OUTSOURCE_SERVICE_TYPE_PREWEDDINGSUITE = 5;
        public static readonly int OUTSOURCE_SERVICE_TYPE_PHOTOBOOK = 6;

        public static readonly int OUTPUT_SERVICE_STATUS_NEW = 1;
        public static readonly int OUTPUT_SERVICE_STATUS_CONFIRM = 2;
        public static readonly int OUTPUT_SERVICE_STATUS_COMPLETE = 3;
        public static readonly int OUTPUT_SERVICE_STATUS_CANCEL = 4;

        public static readonly int LOCATION_SERVICE_TYPE_HOTEL = 1;
        public static readonly int LOCATION_SERVICE_TYPE_PUBLIC = 2;
        public static readonly int LOCATION_SERVICE_TYPE_NOTPUBLIC = 3;

        public static readonly int LOCATION_STATUS_VACANT = 1;
        public static readonly int LOCATION_STATUS_NOTVACANT = 2;

        public static readonly int EQUIPMENT_TYPE_CAMERABODY = 1;
        public static readonly int EQUIPMENT_TYPE_LENS = 2;
        public static readonly int EQUIPMENT_TYPE_ACCESSORY = 3;
        public static readonly int EQUIPMENT_TYPE_LIGHT = 4;
        public static readonly int EQUIPMENT_TYPE_COMPUTER = 5;

        public static readonly int LOCATION_STYLES_OUTDOORS = 2;

        public static readonly int EQUIPMENT_STATUS_VACANT = 1;
        public static readonly int EQUIPMENT_STATUS_NOTVACANT = 2;
        public static readonly int EQUIPMENT_STATUS_MAINTAINENCE = 3;

        public static readonly int EQUIPMENT_SET_1 = 3;
        public static readonly int EQUIPMENT_SET_2 = 5;
        public static readonly int EQUIPMENT_SET_3 = 6;

        public static readonly int EMPLOYEE_STAUS_PENDING_USER = 1;
        public static readonly int EMPLOYEE_STAUS_ACTIVE_USER = 2;
        public static readonly int EMPLOYEE_STAUS_TEMPORARY_DEACTIVE_USER = 3;
        public static readonly int EMPLOYEE_STAUS_DEACTIVE_USER = 4;

        public static readonly int CRM_SERVICE_TYPE_OVERALL = 0;
        public static readonly int CRM_SERVICE_TYPE_PREWEDDING = 1;
        public static readonly int CRM_SERVICE_TYPE_ENGAGEMENT = 2;
        public static readonly int CRM_SERVICE_TYPE_WEDDING = 3;

        public static readonly int CRM_SERVICE_CATEGORY_OVERALL = 0;
        public static readonly int CRM_SERVICE_CATEGORY_PHOTOGRAPH = 1;
        public static readonly int CRM_SERVICE_CATEGORY_CAMERAMAN = 2;
        public static readonly int CRM_SERVICE_CATEGORY_LOCATION = 3;
        public static readonly int CRM_SERVICE_CATEGORY_OUTSOURCE = 4;
        public static readonly int CRM_SERVICE_CATEGORY_OUTPUT = 5;

        public static readonly int LOCATION_STATUS_OPENED = 1;
        public static readonly int LOCATION_STATUS_MOREINFO = 2;
        public static readonly int LOCATION_STATUS_CLOSED = 3;

        public static readonly int EMPLOYEE_POSITION_SALE = 1;
        public static readonly int EMPLOYEE_POSITION_PHOTOGRAPH = 2;
        public static readonly int EMPLOYEE_POSITION_CAMERAMAN = 3;
        public static readonly int EMPLOYEE_POSITION_MANAGER = 4;
        public static readonly int EMPLOYEE_POSITION_ADMIN = 5;

        public static readonly int CUBE_SERVICE_CATEGORY_PHOTOGRAPH = 1;
        public static readonly int CUBE_SERVICE_CATEGORY_CAMERAMAN = 2;
        public static readonly int CUBE_SERVICE_CATEGORY_MEDIA = 3;

        public static readonly int CUBE_SERVICE_TYPE_PREWEDDING = 1;
        public static readonly int CUBE_SERVICE_TYPE_ENGAGEMENT = 2;
        public static readonly int CUBE_SERVICE_TYPE_WEDDING = 3;

        public static readonly int CUBE_PROMOTION_TYPE_1 = 1;
        public static readonly int CUBE_PROMOTION_TYPE_2 = 2;
        public static readonly int CUBE_PROMOTION_TYPE_3 = 3;

        public static readonly string SERVICE_FORM_STATUS_NEW = "รายการใหม่";
        public static readonly string SERVICE_FORM_STATUS_PARTIAL_NEW = "ยังมีรายการที่ไม่ได้รับการยืนยัน";
        public static readonly string SERVICE_FORM_STATUS_CONFIRM = "ยืนยัน";
        public static readonly string SERVICE_FORM_STATUS_PARTIAL_FINISH = "ยังมีบางรายการที่ยังไม่เสร็จสิ้น";
        public static readonly string SERVICE_FORM_STATUS_FINISH = "เสร็จสิ้นการให้บริการ";
        public static readonly string SERVICE_FORM_STATUS_PARTIAL_CANCEL = "มีบางรายการถูกยกเลิกการให้บริการ";
        public static readonly string SERVICE_FORM_STATUS_CANCEL = "ยกเลิกการให้บริการ";

        public static readonly int SERVICE_PHOTOGRAPH = 1;
        public static readonly int SERVICE_EQUIPMENT = 2;
        public static readonly int SERVICE_LOCATION = 3;
        public static readonly int SERVICE_OUTSOURCE = 4;
        public static readonly int SERVICE_OUTPUT = 5;

    }

    public static class RazorExtensions
    {
        public static HelperResult List<T>(this IEnumerable<T> items,
          Func<T, HelperResult> template)
        {
            return new HelperResult(writer =>
            {
                foreach (var item in items)
                {
                    template(item).WriteTo(writer);
                }
            });
        }
    }
}