import DialogItemsType from "./DialogItemsEnum";

interface IDataDialogProp{
    handleClose: () =>void;
    openDialog: boolean;
    dialogTitle: string;
    itemsInDialogType: DialogItemsType;
    username: string;
}
export default IDataDialogProp;