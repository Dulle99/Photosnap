import { Box, Button, Typography } from "@mui/material";
import CategoryItemType from "../../../../../Types/CategoryType/CategoryItemType";

function CategoryItem(prop: CategoryItemType) {
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