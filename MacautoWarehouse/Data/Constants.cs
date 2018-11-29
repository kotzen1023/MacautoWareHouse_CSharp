using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MacautoWarehouse.Data
{
    public class Constants
    {
        public static string SOAP_CONNECTION_FAIL = "com.macauto.MacautoWarehoouse.SoapConnectionFail";

        //login
        public static string ACTION_LOGIN_FAIL = "com.macauto.MacautoWarehoouse.Login.Fail";
        public static string ACTION_LOGIN_SUCCESS = "com.macauto.MacautoWarehoouse.Login.Success";
        public static string ACTION_LOGOUT_ACTION = "com.macauto.MacautoWarehoouse.LogoutAction";
        public static string ACTION_SOCKET_TIMEOUT = "com.macauto.MacautoWarehoouse.SocketTimeOut";
        //
        public static string ACTION_SHOW_VIRTUAL_KEYBOARD_ACTION = "com.macauto.MacautoWarehoouse.ShowVirtualKeyboardAction";
        public static string ACTION_HIDE_VIRTUAL_KEYBOARD_ACTION = "com.macauto.MacautoWarehoouse.HideVirtualKeyboardAction";
        //Main
        public static string ACTION_MAIN_RESET_TITLE = "com.macauto.MacautoWarehoouse.MainResetTitleAction";

        //setting
        public static string ACTION_SETTING_PDA_TYPE_ACTION = "com.macauto.MacautoWarehoouse.SettingPdaTypeAction";
        public static string ACTION_SETTING_WEB_SOAP_PORT_ACTION = "com.macauto.MacautoWarehoouse.SettingWebSoapPortAction";

        //into stock
        public static string ACTION_CHECK_EMP_EXIST_ACTION = "com.macauto.MacautoWarehoouse.CheckEmpExistAction";
        public static string ACTION_CHECK_EMP_EXIST_SUCCESS = "com.macauto.MacautoWarehoouse.CheckEmpExistSuccess";
        public static string ACTION_CHECK_EMP_EXIST_NOT_EXIST = "com.macauto.MacautoWarehoouse.CheckEmpExistNotExist";
        public static string ACTION_CHECK_EMP_PASSWORD_ACTION = "com.macauto.MacautoWarehoouse.CheckEmpPasswordAction";
        public static string ACTION_CHECK_EMP_PASSWORD_SUCCESS = "com.macauto.MacautoWarehoouse.CheckEmpPasswordSuccess";
        public static string ACTION_CHECK_EMP_PASSWORD_FAILED = "com.macauto.MacautoWarehoouse.CheckEmpPasswordFailed";
        public static string ACTION_SET_INSPECTED_RECEIVE_ITEM_CLEAN = "com.macauto.MacautoWarehoouse.SetInspectedReceiveItemClean";
        public static string ACTION_SET_INSPECTED_RECEIVE_ITEM_CLEAN_ONLY = "com.macauto.MacautoWarehoouse.SetInspectedReceiveItemCleanOnly";
        public static string ACTION_GET_INSPECTED_RECEIVE_ITEM_SUCCESS = "com.macauto.MacautoWarehoouse.GetInspectedReceiveItemSuccess";
        public static string ACTION_GET_INSPECTED_RECEIVE_ITEM_EMPTY = "com.macauto.MacautoWarehoouse.GetInspectedReceiveItemEmpty";
        public static string ACTION_GET_INSPECTED_RECEIVE_ITEM_FAILED = "com.macauto.MacautoWarehoouse.GetInspectedReceiveItemFailed";
        public static string ACTION_MODIFIED_ITEM_COMPLETE = "com.macauto.MacautoWarehoouse.ModifiedItemComplete";
        public static string ACTION_CONFIRM_ENTERING_WAREHOUSE_ACTION = "com.macauto.MacautoWarehoouse.ConfirmEnteringWarehouse";
        public static string ACTION_UPDATE_TT_RECEIVE_IN_RVV33_FAILED = "com.macauto.MacautoWarehoouse.UpdateTTReceiveInEvv33Failed";
        public static string ACTION_UPDATE_TT_RECEIVE_IN_RVV33_SUCCESS = "com.macauto.MacautoWarehoouse.UpdateTTReceiveInEvv33Success";
        public static string ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_ACTION = "com.macauto.MacautoWarehoouse.GetDocTypeIsRegOrSub";
        public static string ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_SUCCESS = "com.macauto.MacautoWarehoouse.GetDocTypeIsRegOrSubSuccess";
        public static string ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_FAILED = "com.macauto.MacautoWarehoouse.GetDocTypeIsRegOrSubFailed";
        public static string ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_COMPLETE = "com.macauto.MacautoWarehoouse.GetDocTypeIsRegOrSubComplete";
        public static string ACTION_EXECUTE_TT_ACTION = "com.macauto.MacautoWarehoouse.ExcuteTtAction";
        public static string ACTION_EXECUTE_TT_SUCCESS = "com.macauto.MacautoWarehoouse.ExcuteTtSuccess";
        public static string ACTION_EXECUTE_TT_FAILED = "com.macauto.MacautoWarehoouse.ExcuteTtFailed";
        public static string ACTION_ENTERING_WAREHOUSE_COMPLETE = "com.macauto.MacautoWarehoouse.EnteringWarehouseComplete";
        public static string ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP_ACTION = "com.macauto.MacautoWarehoouse.DeleteTTReceiveGoodsInTempAction";
        public static string ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP_SUCCESS = "com.macauto.MacautoWarehoouse.DeleteTTReceiveGoodsInTempSuccess";
        public static string ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP_FAILED = "com.macauto.MacautoWarehoouse.DeleteTTReceiveGoodsInTempFailed";

        public static string ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_ACTION = "com.macauto.MacautoWarehoouse.DeleteTTReceiveGoodsInTemp2Action";
        public static string ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_SUCCESS = "com.macauto.MacautoWarehoouse.DeleteTTReceiveGoodsInTemp2Success";
        public static string ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_FAILED = "com.macauto.MacautoWarehoouse.DeleteTTReceiveGoodsInTemp2Failed";

        public static string ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_TEXT_CHANGE = "com.macauto.MacautoWarehoouse.EnteringWarehouseDividedDialogTextChange";
        public static string ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_ADD = "com.macauto.MacautoWarehoouse.EnteringWarehouseDividedDialogAdd";
        public static string ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_SHOW = "com.macauto.MacautoWarehoouse.EnteringWarehouseDividedDialogShow";


        public static string ACTION_GET_INSPECTED_RECEIVE_ITEM_AX_ACTION = "com.macauto.MacautoWarehoouse.GetInspectedReceiveItemAXAction";
        public static string ACTION_GET_INSPECTED_RECEIVE_ITEM_AX_SUCCESS = "com.macauto.MacautoWarehoouse.GetInspectedReceiveItemAXSuccess";
        public static string ACTION_GET_INSPECTED_RECEIVE_ITEM_AX_EMPTY = "com.macauto.MacautoWarehoouse.GetInspectedReceiveItemAXEmpty";
        public static string ACTION_GET_INSPECTED_RECEIVE_ITEM_AX_FAILED = "com.macauto.MacautoWarehoouse.GetInspectedReceiveItemAXFailed";

        //locate split
        public static string ACTION_GET_TT_SPLIT_RVV_ITEM_ACTION = "com.macauto.MacautoWarehoouse.GetTTSplitRvvItemAction";
        public static string ACTION_GET_TT_SPLIT_RVV_ITEM_SUCCESS = "com.macauto.MacautoWarehoouse.GetTTSplitRvvItemSuccess";
        public static string ACTION_GET_TT_SPLIT_RVV_ITEM_FAILED = "com.macauto.MacautoWarehoouse.GetTTSplitRvvItemFailed";

        //receiving goods
        public static string ACTION_RECEIVING_GOODS_DATA_ACTION = "com.macauto.MacautoWarehoouse.ReceivingGoodsData";
        public static string ACTION_RECEIVING_GOODS_DATA_FAILED = "com.macauto.MacautoWarehoouse.ReceivingGoodsDataFailed";
        public static string ACTION_RECEIVING_GOODS_DATA_SUCCESS = "com.macauto.MacautoWarehoouse.ReceivingGoodsDataSuccess";
        public static string ACTION_RECEIVING_GOODS_DATA_NO_VENDOR_DATA = "com.macauto.MacautoWarehoouse.ReceivingGoodsDataNoVendorData";

        //allocation
        public static string ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_ACTION = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetLocateNoAction";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetLocateNoSuccess";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_EMPTY = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetLocateNoEmpty";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_FAILED = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetLocateNoFailed";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_ACTION = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetMadeInfoAction";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetMadeInfoSuccess";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_EMPTY = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetMadeInfoEmpty";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_FAILED = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetMadeInfoFailed";
        public static string ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_ACTION = "com.macauto.MacautoWarehoouse.AllocationSendMsgCheckMadeNoExistAction";
        public static string ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationSendMsgCheckMadeNoExistSuccess";
        public static string ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_NOT_EXIST = "com.macauto.MacautoWarehoouse.AllocationSendMsgCheckMadeNoExistNotExist";
        public static string ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_FAILED = "com.macauto.MacautoWarehoouse.AllocationSendMsgCheckStockNoExistFailed";
        public static string ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_ACTION = "com.macauto.MacautoWarehoouse.AllocationSendMsgCheckStockNoExistAction";
        public static string ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationSendMsgCheckStockNoExistSuccess";
        public static string ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_NOT_EXIST = "com.macauto.MacautoWarehoouse.AllocationSendMsgCheckStockNoExistNotExist";
        public static string ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_FAILED = "com.macauto.MacautoWarehoouse.AllocationSendMsgCheckStockNoExistFailed";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_ACTION = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetSfaMessAction";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetSfaMessSuccess";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_EMPTY = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetSfaMessEmpty";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_FAILED = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetSfaMessFailed";
        public static string ACTION_ALLOCATION_GET_TAG_ID_ACTION = "com.macauto.MacautoWarehoouse.AllocationGetTagIdAction";
        public static string ACTION_ALLOCATION_GET_TAG_ID_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationGetTagIdSuccess";
        public static string ACTION_ALLOCATION_GET_TAG_ID_FAILED = "com.macauto.MacautoWarehoouse.AllocationGetTagIdFailed";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_ACTION = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetSfaMessMoveAction";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetSfaMessMoveSuccess";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_EMPTY = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetSfaMessMoveEmpty";
        public static string ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_FAILED = "com.macauto.MacautoWarehoouse.AllocationSendMsgGetSfaMessMoveFailed";
        public static string ACTION_ALLOCATION_HANDLE_MSG_DELETE_ACTION = "com.macauto.MacautoWarehoouse.AllocationHandleMsgDeleteAction";
        public static string ACTION_ALLOCATION_HANDLE_MSG_DELETE_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationHandleMsgDeleteSuccess";
        public static string ACTION_ALLOCATION_HANDLE_MSG_DELETE_FAILED = "com.macauto.MacautoWarehoouse.AllocationHandleMsgDeleteFailed";

        public static string ACTION_ALLOCATION_GET_MY_MESS_LIST_ACTION = "com.macauto.MacautoWarehoouse.AllocationGetMyMessListAction";
        public static string ACTION_ALLOCATION_GET_MY_MESS_LIST_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationGetMyMessListSuccess";
        public static string ACTION_ALLOCATION_GET_MY_MESS_LIST_EMPTY = "com.macauto.MacautoWarehoouse.AllocationGetMyMessListEmpty";
        public static string ACTION_ALLOCATION_GET_MY_MESS_LIST_FAILED = "com.macauto.MacautoWarehoouse.AllocationGetMyMessListFailed";
        public static string ACTION_ALLOCATION_GET_MY_MESS_DETAIL_ACTION = "com.macauto.MacautoWarehoouse.AllocationGetMyMessDetailAction";
        public static string ACTION_ALLOCATION_GET_MY_MESS_DETAIL_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationGetMyMessDetailSuccess";
        public static string ACTION_ALLOCATION_GET_MY_MESS_DETAIL_FAILED = "com.macauto.MacautoWarehoouse.AllocationGetMyMessDetailFailed";
        public static string ACTION_ALLOCATION_CHECK_IS_DELETE_RIGHT_ACTION = "com.macauto.MacautoWarehoouse.AllocationCheckIsDeleteRightAction";
        public static string ACTION_ALLOCATION_CHECK_IS_DELETE_RIGHT_YES = "com.macauto.MacautoWarehoouse.AllocationCheckIsDeleteRightYes";
        public static string ACTION_ALLOCATION_CHECK_IS_DELETE_RIGHT_NO = "com.macauto.MacautoWarehoouse.AllocationCheckIsDeleteRightNo";
        public static string ACTION_ALLOCATION_CHECK_IS_DELETE_RIGHT_FAILED = "com.macauto.MacautoWarehoouse.AllocationCheckIsDeleteRightFailed";
        public static string ACTION_ALLOCATION_GET_DEPT_NO_ACTION = "com.macauto.MacautoWarehoouse.AllocationGetDeptNoAction";
        public static string ACTION_ALLOCATION_GET_DEPT_NO_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationGetDeptNoSuccess";
        public static string ACTION_ALLOCATION_GET_DEPT_NO_FAILED = "com.macauto.MacautoWarehoouse.AllocationGetDeptNoFailed";
        public static string ACTION_ALLOCATION_GET_NEW_DOC_NO_ACTION = "com.macauto.MacautoWarehoouse.AllocationGetNewDocNoAction";
        public static string ACTION_ALLOCATION_GET_NEW_DOC_NO_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationGetNewDocNoSuccess";
        public static string ACTION_ALLOCATION_GET_NEW_DOC_NO_FAILED = "com.macauto.MacautoWarehoouse.AllocationGetNewDocNoFailed";
        public static string ACTION_ALLOCATION_INSERT_TT_IMN_FILE_NO_TLF_NO_IMG_ACTION = "com.macauto.MacautoWarehoouse.AllocationInsertTTImnFileNoTlfNoImgAction";
        public static string ACTION_ALLOCATION_INSERT_TT_IMN_FILE_NO_TLF_NO_IMG_YES = "com.macauto.MacautoWarehoouse.AllocationInsertTTImnFileNoTlfNoImgYes";
        public static string ACTION_ALLOCATION_INSERT_TT_IMN_FILE_NO_TLF_NO_IMG_NO = "com.macauto.MacautoWarehoouse.AllocationInsertTTImnFileNoTlfNoImgNo";
        public static string ACTION_ALLOCATION_INSERT_TT_IMN_FILE_NO_TLF_NO_IMG_FAILED = "com.macauto.MacautoWarehoouse.AllocationInsertTTImnFileNoTlfNoImgFailed";


        public static string ACTION_GET_PART_NO_NEED_SCAN_STATUS_ACTION = "com.macauto.MacautoWarehoouse.AllocationGetPartNoNeedScanStatusAction";
        public static string ACTION_GET_PART_NO_NEED_SCAN_STATUS_YES = "com.macauto.MacautoWarehoouse.AllocationGetPartNoNeedScanStatusYes";
        public static string ACTION_GET_PART_NO_NEED_SCAN_STATUS_NO = "com.macauto.MacautoWarehoouse.AllocationGetPartNoNeedScanStatusNo";
        public static string ACTION_GET_PART_NO_NEED_SCAN_STATUS_FAILED = "com.macauto.MacautoWarehoouse.AllocationGetPartNoNeedScanStatusFailed";

        //barcode
        public static string ACTION_ALLOCATION_GET_LOT_CODE_ACTION = "com.macauto.MacautoWarehoouse.AllocationGetLotCodeAction";
        public static string ACTION_ALLOCATION_GET_LOT_CODE_EMPTY = "com.macauto.MacautoWarehoouse.AllocationGetLotCodeEmpty";
        public static string ACTION_ALLOCATION_GET_LOT_CODE_SUCCESS = "com.macauto.MacautoWarehoouse.AllocationGetLotCodeSuccess";
        public static string ACTION_ALLOCATION_GET_LOT_CODE_FAILED = "com.macauto.MacautoWarehoouse.AllocationGetLotCodeFailed";
        //swipe layout
        public static string ACTION_ALLOCATION_SWIPE_LAYOUT_UPDATE = "com.macauto.MacautoWarehoouse.AllocationSwipeLayoutUpdate";
        public static string ACTION_ALLOCATION_SWIPE_LAYOUT_DELETE_ROW = "com.macauto.MacautoWarehoouse.AllocationSwipeLayoutDeleteRow";
        //delete
        public static string ACTION_ALLOCATION_SEND_MSG_DELETE_ITEM_CONFIRM = "com.macauto.MacautoWarehoouse.AllocationSendMsgDeleteItemConfirm";
        public static string ACTION_ALLOCATION_MSG_DETAIL_DELETE_ITEM_CONFIRM = "com.macauto.MacautoWarehoouse.AllocationMsgDetailDeleteItemConfirm";


        //batch_no
        public static string ACTION_SEARCH_PART_BATCH_ACTION = "com.macauto.MacautoWarehoouse.SearchPartWarehouseBatchAction";
        public static string ACTION_SEARCH_PART_BATCH_CLEAN = "com.macauto.MacautoWarehoouse.SearchPartWarehouseBatchClean";
        public static string ACTION_SEARCH_PART_BATCH_FAILED = "com.macauto.MacautoWarehoouse.SearchPartWarehouseBatchFailed";
        public static string ACTION_SEARCH_PART_BATCH_SUCCESS = "com.macauto.MacautoWarehoouse.SearchPartWarehouseBatchSuccess";

        //Search Parts
        public static string ACTION_SEARCH_PART_WAREHOUSE_LIST_ACTION = "com.macauto.MacautoWarehoouse.SearchPartWarehouseListAction";
        public static string ACTION_SEARCH_PART_WAREHOUSE_LIST_CLEAN = "com.macauto.MacautoWarehoouse.SearchPartWarehouseListClean";
        public static string ACTION_SEARCH_PART_WAREHOUSE_LIST_FAILED = "com.macauto.MacautoWarehoouse.SearchPartWarehouseListFailed";
        public static string ACTION_SEARCH_PART_WAREHOUSE_LIST_SUCCESS = "com.macauto.MacautoWarehoouse.SearchPartWarehouseListSuccess";
        public static string ACTION_SEARCH_PART_WAREHOUSE_LIST_EMPTY = "com.macauto.MacautoWarehoouse.SearchPartWarehouseListEmpty";
        public static string ACTION_SEARCH_PART_WAREHOUSE_SORT_COMPLETE = "com.macauto.MacautoWarehoouse.SearchPartWarehouseSortComplete";
        public static string ACTION_SEARCH_PART_WAREHOUSE_GET_ORIGINAL_LIST = "com.macauto.MacautoWarehoouse.SearchPartWarehouseGetOriginalList";

        //Shipment
        public static string ACTION_SHIPMENT_GET_PRE_SHIPPING_INFO_ACTION = "com.macauto.MacautoWarehoouse.ShipmentGetPreShippingInfoAction";
        public static string ACTION_SHIPMENT_GET_PRE_SHIPPING_INFO_FAILED = "com.macauto.MacautoWarehoouse.ShipmentGetPreShippingInfoFailed";
        public static string ACTION_SHIPMENT_GET_PRE_SHIPPING_INFO_SUCCESS = "com.macauto.MacautoWarehoouse.ShipmentGetPreShippingInfoSuccess";
        public static string ACTION_SHIPMENT_GET_PRE_SHIPPING_INFO_EMPTY = "com.macauto.MacautoWarehoouse.ShipmentGetPreShippingInfoEmpty";

        public static string ACTION_SHIPMENT_GET_OGC_FILE_2_ACTION = "com.macauto.MacautoWarehoouse.ShipmentGetOgcFile2Action";
        public static string ACTION_SHIPMENT_GET_OGC_FILE_2_FAILED = "com.macauto.MacautoWarehoouse.ShipmentGetOgcFile2Failed";
        public static string ACTION_SHIPMENT_GET_OGC_FILE_2_SUCCESS = "com.macauto.MacautoWarehoouse.ShipmentGetOgcFile2Success";
        public static string ACTION_SHIPMENT_GET_OGC_FILE_2_EMPTY = "com.macauto.MacautoWarehoouse.ShipmentGetOgcFile2Empty";

        public static string ACTION_SHIPMENT_GET_WAREHOUSE_ACTION = "com.macauto.MacautoWarehoouse.ShipmentGetWarehouseAction";
        public static string ACTION_SHIPMENT_GET_WAREHOUSE_FAILED = "com.macauto.MacautoWarehoouse.ShipmentGetWarehouseFailed";
        public static string ACTION_SHIPMENT_GET_WAREHOUSE_SUCCESS = "com.macauto.MacautoWarehoouse.ShipmentGetWarehouseSuccess";
        public static string ACTION_SHIPMENT_GET_WAREHOUSE_EMPTY = "com.macauto.MacautoWarehoouse.ShipmentGetWarehouseEmpty";

        public static string ACTION_SHIPMENT_GET_PRE_SHIPPING_INFO_CONFIRM_SP_ACTION = "com.macauto.MacautoWarehoouse.ShipmentGetPreShippingInfoConfirmSpAction";
        public static string ACTION_SHIPMENT_GET_PRE_SHIPPING_INFO_CONFIRM_SP_FAILED = "com.macauto.MacautoWarehoouse.ShipmentGetPreShippingInfoConfirmSpFailed";
        public static string ACTION_SHIPMENT_GET_PRE_SHIPPING_INFO_CONFIRM_SP_SUCCESS = "com.macauto.MacautoWarehoouse.ShipmentGetPreShippingInfoConfirmSpSuccess";

        public static string ACTION_SHIPMENT_SHIPPING_INSERT_OGC_FILE_ACTION = "com.macauto.MacautoWarehoouse.ShipmentShippingInsertOgcFileAction";
        public static string ACTION_SHIPMENT_SHIPPING_INSERT_OGC_FILE_FAILED = "com.macauto.MacautoWarehoouse.ShipmentShippingInsertOgcFileFailed";
        public static string ACTION_SHIPMENT_SHIPPING_INSERT_OGC_FILE_SUCCESS = "com.macauto.MacautoWarehoouse.ShipmentShippingInsertOgcFileSuccess";

        public static string ACTION_SCAN_RESET = "com.macauto.MacautoWarehoouse.ScanReset";
        public static string ACTION_CHECK_STOCK_LOCATE_NO_EXIST_ACTION = "com.macauto.MacautoWarehoouse.CheckStockLocateNoExist";

        public static string ACTION_SCAN = "com.macauto.MacautoWarehoouse.client.android.SCAN";
        public static string ACTION_GET_BARCODE_MESSGAGE_COMPLETE = "com.macauto.MacautoWarehoouse.GetBarcodeMessageComplete";
        public static string ACTION_CHECK_RECEIVE_GOODS = "com.macauto.MacautoWarehoouse.CheckReceiveGoods.Action";

        public static string ACTION_SEARCH_MENU_SHOW_ACTION = "com.macauto.MacautoWarehoouse.SearchMenuShowAction";
        public static string ACTION_SEARCH_MENU_HIDE_ACTION = "com.macauto.MacautoWarehoouse.SearchMenuHideAction";
        public static string ACTION_RESET_TITLE_PART_IN_STOCK = "com.macauto.MacautoWarehoouse.ResetTitlePartInStock";

        //Production Storage
        public static string ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_ACTION = "com.macauto.MacautoWarehoouse.CheckTTProductEntryAlreadyConfirmAction";
        public static string ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_FAILED = "com.macauto.MacautoWarehoouse.CheckTTProductEntryAlreadyConfirmFailed";
        public static string ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_YES = "com.macauto.MacautoWarehoouse.CheckTTProductEntryAlreadyConfirmYes";
        public static string ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_NO = "com.macauto.MacautoWarehoouse.CheckTTProductEntryAlreadyConfirmNo";

        public static string ACTION_GET_TT_PRODUCT_ENTRY_ACTION = "com.macauto.MacautoWarehoouse.GetTTProductEntryAction";
        public static string ACTION_GET_TT_PRODUCT_ENTRY_FAILED = "com.macauto.MacautoWarehoouse.GetTTProductEntryFailed";
        public static string ACTION_GET_TT_PRODUCT_ENTRY_SUCCESS = "com.macauto.MacautoWarehoouse.GetTTProductEntrySuccess";
        public static string ACTION_GET_TT_PRODUCT_ENTRY_EMPTY = "com.macauto.MacautoWarehoouse.GetTTProductEntryEmpty";

        public static string ACTION_PRODUCT_CHECK_STOCK_LOCATE_NO_EXIST_ACTION = "com.macauto.MacautoWarehoouse.ProductCheckStockLocateNoExistAction";
        public static string ACTION_PRODUCT_CHECK_STOCK_LOCATE_NO_EXIST_FAILED = "com.macauto.MacautoWarehoouse.ProductCheckStockLocateNoExistFailed";
        public static string ACTION_PRODUCT_CHECK_STOCK_LOCATE_NO_EXIST_YES = "com.macauto.MacautoWarehoouse.ProductCheckStockLocateNoExistYes";
        public static string ACTION_PRODUCT_CHECK_STOCK_LOCATE_NO_EXIST_NO = "com.macauto.MacautoWarehoouse.ProductCheckStockLocateNoExistNo";

        public static string ACTION_PRODUCT_UPDATE_TT_PRODUCT_ENTRY_LOCATE_NO_ACTION = "com.macauto.MacautoWarehoouse.ProductUpdateTTProductEntryLocateNoAction";
        public static string ACTION_PRODUCT_UPDATE_TT_PRODUCT_ENTRY_LOCATE_NO_FAILED = "com.macauto.MacautoWarehoouse.ProductUpdateTTProductEntryLocateNoFailed";
        public static string ACTION_PRODUCT_UPDATE_TT_PRODUCT_ENTRY_LOCATE_NO_SUCCESS = "com.macauto.MacautoWarehoouse.ProductUpdateTTProductEntryLocateNoSuccess";

        public static string ACTION_PRODUCT_IN_STOCK_WORK_COMPLETE = "com.macauto.MacautoWarehoouse.ProductInStockWorkComplete";
        public static string ACTION_PRODUCT_SWIPE_LAYOUT_UPDATE = "com.macauto.MacautoWarehoouse.ProductSwipeLayoutUpdate";
        public static string ACTION_PRODUCT_DELETE_ITEM_CONFIRM = "com.macauto.MacautoWarehoouse.ProductDeleteItemConfirm";

        public static string ACTION_GET_TT_ERROR_STATUS_ACTION = "com.macauto.MacautoWarehoouse.GetTTErrorStatusAction";
        public static string ACTION_GET_TT_ERROR_STATUS_FAILED = "com.macauto.MacautoWarehoouse.GetTTErrorStatusFailed";
        public static string ACTION_GET_TT_ERROR_STATUS_SUCCESS = "com.macauto.MacautoWarehoouse.GetTTErrorStatusSuccess";

        //Receiving Inspection
        public static string ACTION_RECEIVING_INSPECTION_GET_TT_RECEIVE_GOODS_REPORT_DATA_QC_ACTION = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTReceiveGoodsReportDataQCAction";
        public static string ACTION_RECEIVING_INSPECTION_GET_TT_RECEIVE_GOODS_REPORT_DATA_QC_SUCCESS = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTReceiveGoodsReportDataQCSuccess";
        public static string ACTION_RECEIVING_INSPECTION_GET_TT_RECEIVE_GOODS_REPORT_DATA_QC_EMPTY = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTReceiveGoodsReportDataQCEmpty";
        public static string ACTION_RECEIVING_INSPECTION_GET_TT_RECEIVE_GOODS_REPORT_DATA_QC_FAILED = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTReceiveGoodsReportDataQCFailed";

        public static string ACTION_RECEIVING_INSPECTION_GET_TT_REC_NO_IN_QC_ACTION = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTRecNoInQCAction";
        public static string ACTION_RECEIVING_INSPECTION_GET_TT_REC_NO_IN_QC_YES = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTRecNoInQCYes";
        public static string ACTION_RECEIVING_INSPECTION_GET_TT_REC_NO_IN_QC_NO = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTRecNoInQCNo";
        public static string ACTION_RECEIVING_INSPECTION_GET_TT_REC_NO_IN_QC_COMPLETE = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTRecNoInQCComplete";
        public static string ACTION_RECEIVING_INSPECTION_GET_TT_REC_NO_IN_QC_FAILED = "com.macauto.MacautoWarehoouse.ReceivingInspectionGetTTRecNoInQCFailed";

        public static string ACTION_RECEIVING_INSPECTION_EXECUTE_TT_OQCP_ACTION = "com.macauto.MacautoWarehoouse.ReceivingInspectionExecuteTTOQCPAction";
        public static string ACTION_RECEIVING_INSPECTION_EXECUTE_TT_OQCP_SUCCESS = "com.macauto.MacautoWarehoouse.ReceivingInspectionExecuteTTOQCPSuccess";
        public static string ACTION_RECEIVING_INSPECTION_EXECUTE_TT_OQCP_FAILED = "com.macauto.MacautoWarehoouse.ReceivingInspectionExecuteTTOQCPFailed";

        public static string ACTION_RECEIVING_INSPECTION_EXECUTE_TT_PRG_A_ACTION = "com.macauto.MacautoWarehoouse.ReceivingInspectionExecuteTTPrgAAction";
        public static string ACTION_RECEIVING_INSPECTION_EXECUTE_TT_PRG_A_SUCCESS = "com.macauto.MacautoWarehoouse.ReceivingInspectionExecuteTTPrgASuccess";
        public static string ACTION_RECEIVING_INSPECTION_EXECUTE_TT_PRG_A_FAILED = "com.macauto.MacautoWarehoouse.ReceivingInspectionExecuteTTPrgAFailed";

        public static string ACTION_RECEIVING_INSPECTION_DELETE_QC_TEMP_ACTION = "com.macauto.MacautoWarehoouse.ReceivingInspectionDeleteQCTempAction";
        public static string ACTION_RECEIVING_INSPECTION_DELETE_QC_TEMP_SUCCESS = "com.macauto.MacautoWarehoouse.ReceivingInspectionDeleteQCTempSuccess";
        public static string ACTION_RECEIVING_INSPECTION_DELETE_QC_TEMP_FAILED = "com.macauto.MacautoWarehoouse.ReceivingInspectionDeleteQCTempFailed";

        public static string ACTION_RECEIVING_INSPECTION_ITEM_SELECT_CHANGE = "com.macauto.MacautoWarehoouse.ReceivingInspectionItemSelectChange";
        //for text change
        public static string ACTION_EDITTEXT_TEXT_CHANGE = "com.macauto.MacautoWarehoouse.EditTextTextChange";
    }
}