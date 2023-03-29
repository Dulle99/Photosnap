import { Box, Button, Typography } from "@mui/material";
import CategoryItemType from "../../../../../Types/CategoryType/CategoryItemType";

function CategoryItem(prop: CategoryItemType) {
    //TODO: Add on click handler to open clicked user profile
    return (
        <Box>
            <Button sx={{
                background: prop.categoryColor
            }}>
                <Typography marginLeft={1} color={"#FFFFFF"}><strong>{prop.categoryName}</strong></Typography>
            </Button>
        </Box>
    
    );
}

export default CategoryItem;