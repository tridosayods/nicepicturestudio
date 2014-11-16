using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NicePictureStudio.App_Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NicePictureStudio.Models;
using System.Collections;
using NicePictureStudio.Utils;
using System.Text.RegularExpressions;

namespace NicePictureStudio.Utils
{
    public static class ValidateServiceTableClass
    {
        public static bool ValidateStatus(SchedulerViewModels service, ModelStateDictionary modelState, int status = 0)
        {
            if (service.selectedStatus == Constant.SERVICE_STATUS_CONFIRM)
            {
                if ((service.EquipmentStatus == Constant.SERVICE_FORM_STATUS_CONFIRM || service.EquipmentStatus == Constant.SERVICE_FORM_NOSERVICES)
                    && service.PhotoGraphStatus == Constant.SERVICE_FORM_STATUS_CONFIRM
                    && (service.LocationStatus == Constant.SERVICE_FORM_STATUS_CONFIRM || service.LocationStatus == Constant.SERVICE_FORM_NOSERVICES)
                    && (service.OutsourceStatus == Constant.SERVICE_FORM_STATUS_CONFIRM || service.OutsourceStatus == Constant.SERVICE_FORM_NOSERVICES))
                {
                    return true;
                }
                else if ((status <= Constant.SERVICE_STATUS_CONFIRM && status >0) || status==Constant.SERVICE_STATUS_WARNING)
                { 
                    //status is OK to confirm 
                    return true;
                }
                else if (status == 0)
                {
                    modelState.AddModelError("ผิดพลาด", "ยังมีบางรายการที่มีสถานะการให้บริการยังไม่ยืนยันให้ครบ โปรดตรวจสอบ สถานะช่างถ่ายภาพ สถานะพนักงานอุปกรณ์ถ่ายภาพ สถานะของสถานที่ให้บริการ และ สถานะของผู้ให้บริการจัดจ้าง");
                    return false;
                }
                else
                {
                    modelState.AddModelError("ผิดพลาด", "ไม่สารมารถเผลี่ยนแปลงสถานะได้ เนื่องจากสถานะปัจจุบัน มีการแจ้งเตือน หรือถูกยกเลิกไปแล้ว");
                    return false;
                }
            }
            else if (service.selectedStatus == Constant.SERVICE_STATUS_COMPLETE)
            {
                if ((service.EquipmentStatus == Constant.SERVICE_FORM_STATUS_FINISH || service.EquipmentStatus == Constant.SERVICE_FORM_NOSERVICES)
                    && (service.PhotoGraphStatus == Constant.SERVICE_FORM_STATUS_FINISH || service.PhotoGraphStatus == Constant.SERVICE_FORM_NOSERVICES)
                    && (service.LocationStatus == Constant.SERVICE_FORM_STATUS_FINISH || service.LocationStatus == Constant.SERVICE_FORM_NOSERVICES)
                    && (service.OutsourceStatus == Constant.SERVICE_FORM_STATUS_FINISH || service.OutsourceStatus == Constant.SERVICE_FORM_NOSERVICES)
                    && (service.OutputStatus == Constant.SERVICE_FORM_STATUS_FINISH || service.OutputStatus == Constant.SERVICE_FORM_NOSERVICES))
                {
                    return true;
                }
                else if ((status <= Constant.SERVICE_STATUS_COMPLETE && status > 0))
                {
                    //status is OK to confirm 
                    return true;
                }
                else if (status == 0)
                {
                    modelState.AddModelError("ผิดพลาด", "ยังมีบางรายการที่มีสถานะการให้บริการยังไม่เสร็จสิ้นการให้บริการ โปรดตรวจสอบ สถานะช่างถ่ายภาพ สถานะพนักงานอุปกรณ์ถ่ายภาพ สถานะของสถานที่ให้บริการ สถานะของผู้ให้บริการจัดจ้าง หรือ พนักงานฝ่ายมัลติมีเดีย");
                    return false;
                }
                else 
                {
                    modelState.AddModelError("ผิดพลาด", "ไม่สารมารถเผลี่ยนแปลงสถานะได้ เนื่องจากสถานะปัจจุบัน อาจมีการยกเลิกไปแล้ว");
                    return false;
                }

            }
            else if (service.selectedStatus == Constant.SERVICE_STATUS_CANCEL)
            {
                var now = DateTime.Now.AddDays(7);
                if (service.Start.CompareTo(now) >= 0)
                {
                    return true;
                }
                else
                {
                    modelState.AddModelError("คำเตือน", "ไม่สามารถยกเลิกแบบไม่เสียค่ามัดจำ ได้เนื่องจากระยะเวลาในการให้บริการจะถึงภายในน้อยกว่า 7 วันทำการ จากเวลาปัจจุบัน");
                    return false;
                }
            }
            else if (service.selectedStatus == Constant.SERVICE_STATUS_CANCEL_IN7DAYS)
            {
                var now = DateTime.Now.AddDays(7);
                if (service.Start.CompareTo(now) <= 0)
                {
                    return true;
                }
                else
                {
                    modelState.AddModelError("คำเตือน", "ไม่สามารถยกเลิกแบบหักค้ามัดจำ 50% ได้เนื่องจากระยะเวลาในการให้บริการมีมากกว่า 7 วันทำการ จากเวลาปัจจุบัน");
                    return false;
                }
            }
            else if (service.selectedStatus == Constant.SERVICE_STATUS_NEW)
            {
                modelState.AddModelError("คำเตือน", "ไม่สามารเปลี่ยนสถานะกลับไปเป็น 'รายการใหม่'");
                return false;
            }
            else if (service.selectedStatus == Constant.SERVICE_STATUS_WARNING)
            {
                if (status == 0)
                {
                    modelState.AddModelError("คำเตือน", "ไม่สามารเปลี่ยนสถานะเป็น 'แจ้งเตือนได้' ระบบจะอนุญาติเให้แต้งเตือนได้เฉพาะรายการหลักเท่านั้น");
                    return false;
                }
                else if (status > Constant.SERVICE_STATUS_NEW && status > 0)
                {
                    modelState.AddModelError("คำเตือน", "ไม่สามารเปลี่ยนสถานะเป็น 'แจ้งเตือนได้' เนื่องจากรายการนี้ได้ถูกดำเนินการไปแล้ว");
                    return false;
                }
            }
            return true;
        }


        public static bool ValidateBookingStatus(SchedulerViewModels service, ModelStateDictionary modelState, int status = 0)
        {
            if (service.selectedStatus == Constant.BOOKING_STATUS_NEW)
            {
                modelState.AddModelError("คำเตือน", "ไม่สามารเปลี่ยนสถานะเป็น 'รายการจองใหม่ได้' เนื่องจากรายการนี้ได้ถูกดำเนินการไปแล้ว");
                return false;
            }
            else if (service.selectedStatus == Constant.BOOKING_STATUS_CONFIRM)
            {
                modelState.AddModelError("คำเตือน", "ไม่สามารเปลี่ยนสถานะเป็น 'ยืนยันได้' กรุณาไปยังหน้าสร้างรายการจอง");
                return false;
            }
            else if (service.selectedStatus == Constant.BOOKING_STATUS_OPERATED)
            {
                modelState.AddModelError("คำเตือน", "ไม่สามารเปลี่ยนสถานะเป็น 'เริ่มต้นให้บริการ' กรุณาไปยังส่งนของการสร้างรายการให้บริการ");
                return false;
            }
            else if ((service.selectedStatus == Constant.BOOKING_STATUS_CANCEL))
            {
                if (status > Constant.BOOKING_STATUS_CONFIRM)
                {
                    modelState.AddModelError("ผิดพลาด", "ไม่สามารเปลี่ยนสถานะเป็น 'ยกเลิก' ได้ เนื่องจากลูกค้าได้เข้ามาสร้างรายการให้บริการแล้ว");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else 
            {
                return false;
            }
        }
    }
}