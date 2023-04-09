import { DialogProps } from "@mui/material";
import { useState } from "react";
import DialogItemsType from "../../../Interfaces/UserProfile/DialogProps/DialogItemsEnum";
import IListOfUsersDialog from "../../../Interfaces/UserProfile/DialogProps/IListOfUsersDialog-prop";
import DataDialog from "./ListOfObjectsDialog/DataDialog";
import DialogButton from "./ListOfObjectsDialog/DialogButton";

function ListOfFollowingDialog(props: IListOfUsersDialog) {
    const [open, setOpen] = useState(false);
    
    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };
    
    return (
    <>
        <DialogButton username={props.username} numberOfDataObjects={props.numberOfUsers} dialogButtonName="Following" handleClickOpen={handleClickOpen} />
        <DataDialog handleClose={handleClose} dialogTitle="Following" openDialog={open} 
                    itemsInDialogType={DialogItemsType.listOfFollowings} username={props.username} totalDialogItems={props.numberOfUsers} />
    </>);
}
 
export default ListOfFollowingDialog;