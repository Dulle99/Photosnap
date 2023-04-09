import DialogItemsType from "./DialogItemsEnum";

interface IDataDialogProp{
    handleClose: () =>void;
    openDialog: boolean;
    dialogTitle: string;
    itemsInDialogType: DialogItemsType;
    username: string;
    totalDialogItems: number;
}
export default IDataDialogProp;