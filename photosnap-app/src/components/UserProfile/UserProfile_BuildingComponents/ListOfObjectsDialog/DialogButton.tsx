import Button from '@mui/material/Button';
import { Box, Typography } from '@mui/material';
import IDialogButtonProp from '../../../../Interfaces/UserProfile/DialogProps/IDialogButton-prop';

function DialogButton(prop: IDialogButtonProp) {
    return (
        <>
            <Box sx={{ display: "flex", flexDirection: "column" }} >
                <Button onClick={prop.handleClickOpen} sx={{ flexDirection: "column", textTransform: "none" }}>
                    <Typography color='#020000' ><strong>{prop.numberOfDataObjects}</strong></Typography>
                    <Typography color='#020000'>{prop.dialogButtonName}</Typography>
                </Button>
            </Box>
        </>
    );
}

export default DialogButton;