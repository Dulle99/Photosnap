import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { useEffect, useRef } from 'react';
import IDataDialogProp from '../../../../Interfaces/UserProfile/DialogProps/IDataDialog-prop';
import DialogItems from './DialogItems';


function DataDialog(props: IDataDialogProp){
    const descriptionElementRef = useRef<HTMLElement>(null);
    useEffect(() => {
        if (props.openDialog) {
            const { current: descriptionElement } = descriptionElementRef;
            if (descriptionElement !== null) {
                descriptionElement.focus();
            }
        }
    }, [props.openDialog]);
    return (
        <>
        <Dialog
                open={props.openDialog}
                onClose={props.handleClose}
                scroll={"paper"}
                aria-labelledby="scroll-dialog-title"
                aria-describedby="scroll-dialog-description"
            >
                <DialogTitle id="scroll-dialog-title">{props.dialogTitle}</DialogTitle>
                <DialogContent dividers={true}>
                    <DialogContent
                        id="scroll-dialog-description"
                        ref={descriptionElementRef}
                        tabIndex={-1}
                    >
                        <DialogItems username={props.username} itemsType={props.itemsInDialogType} />
                    </DialogContent>
                </DialogContent>
                <DialogActions>
                    <Button sx={{textTransform:"none", color:"#E65664"}}onClick={props.handleClose}>Close</Button>
                </DialogActions>
            </Dialog>
        </>
      );
}
 
export default DataDialog;