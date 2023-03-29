import { DialogProps } from "@mui/material";
import { useState } from "react";
import DialogItemsType from "../../../Interfaces/UserProfile/DialogProps/DialogItemsEnum";
import IListOfCategoriesProp from "../../../Interfaces/UserProfile/DialogProps/IListOfCategoriesDialog-prop";
import DataDialog from "./ListOfObjectsDialog/DataDialog";
import DialogButton from "./ListOfObjectsDialog/DialogButton";

function ListOfCategories(props: IListOfCategoriesProp) {
    const [open, setOpen] = useState(false);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    return (
        <>
            <DialogButton username={props.username} numberOfDataObjects={props.numberOfCategories} dialogButtonName="Categories" handleClickOpen={handleClickOpen} />
            <DataDialog handleClose={handleClose} dialogTitle="Categories" openDialog={open} itemsInDialogType={DialogItemsType.listOfCategories} username={props.username} />
        </>);
}

export default ListOfCategories;