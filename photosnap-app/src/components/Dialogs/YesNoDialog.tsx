import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { useEffect, useState } from 'react';
import IYesNoDialog from '../../Interfaces/DialogInterfaces/IYesNoDialog';

function YesNoDialog(prop: IYesNoDialog) {
    const [open, setOpen] = useState(true);


    const handleClose: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault();
        if (e.currentTarget.id === "Yes")
            prop.deletePhoto(true);
        else
            prop.deletePhoto(false);
        setOpen(false);
    }
    
    useEffect(() => {
    }, []);

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title">
                    {"Warning"}
                </DialogTitle>
                <DialogContent>
                    <DialogContentText id="alert-dialog-description">
                        {prop.dialogText}
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} id="No">No</Button>
                    <Button onClick={handleClose} id="Yes" autoFocus>
                        Yes
                    </Button>
                </DialogActions>
            </Dialog>
        </div>
    );
}

export default YesNoDialog;