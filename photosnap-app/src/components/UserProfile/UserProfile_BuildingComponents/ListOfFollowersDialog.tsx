import { useState } from "react";
import DialogItemsType from "../../../Interfaces/UserProfile/DialogProps/DialogItemsEnum";
import IListOfUsersDialog from "../../../Interfaces/UserProfile/DialogProps/IListOfUsersDialog-prop";
import DataDialog from "./ListOfObjectsDialog/DataDialog";
import DialogButton from "./ListOfObjectsDialog/DialogButton";

function ListOfFollowersDialog(props: IListOfUsersDialog) {
    const [open, setOpen] = useState(false);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    return (
        <>
            <DialogButton username={props.username} numberOfDataObjects={props.numberOfUser} dialogButtonName="Followers" handleClickOpen={handleClickOpen} />
            <DataDialog handleClose={handleClose} dialogTitle="Followers" openDialog={open} itemsInDialogType={DialogItemsType.listOfFollowers} username={props.username} />
        </>);
}

export default ListOfFollowersDialog;