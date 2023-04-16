import * as React from 'react';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import ListItemText from '@mui/material/ListItemText';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import Checkbox from '@mui/material/Checkbox';
import { useEffect, useState } from 'react';
import CategoryItemType from '../../Types/CategoryType/CategoryItemType';
import axios from 'axios';
import { Chip, Typography } from '@mui/material';
import ISelectedPhotoCategory from '../../Interfaces/PhotoCategory/ISelectPhotoCategory';

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 250,
        },
    },
};

function SelectPhotoCategory(prop: ISelectedPhotoCategory) {
    const [photoCategories, setPhotoCategories] = useState<CategoryItemType[]>([]);
    const [selectedPhotoCategories, setSelectedPhotoCategories] = useState<string[]>([]);

    const handleChange = (event: SelectChangeEvent<typeof selectedPhotoCategories>) => {
        const {
            target: { value },
        } = event;
        prop.setSelectedPhotoCategories(
            // On autofill we get a stringified value.
            typeof value === 'string' ? value.split(',') : value,
        );
        
    };

    useEffect(() => {
        const fetchItems = async () => {
            const result = await axios.get<CategoryItemType[]>('https://localhost:7053/api/PhotoCategory/GetPhotoCategories', {
                headers: {
                    'Authorization': 'Bearer ' + window.sessionStorage.getItem("token")
                }
            });
            setPhotoCategories(result.data);
        }
        fetchItems();
    }, []);

    return (
        <>
            <FormControl sx={{ mt:2, width: 300 }}>
                <InputLabel id="demo-multiple-checkbox-label">Photo categories</InputLabel>
                <Select
                    labelId="demo-multiple-checkbox-label"
                    id="demo-multiple-checkbox"
                    multiple
                    value={prop.selectedPhotoCategories}
                    onChange={handleChange}
                    input={<OutlinedInput label="Photo categories" />}
                    renderValue={(selected) => selected.join(', ')}
                    MenuProps={MenuProps}
                >
                    {photoCategories.map((photoCategory) => (
                        <MenuItem key={photoCategory.categoryName} value={photoCategory.categoryName}>
                            <Checkbox checked={prop.selectedPhotoCategories.indexOf(photoCategory.categoryName) > -1} />
                            <Chip label={photoCategory.categoryName} color='primary' style={{backgroundColor:photoCategory.categoryColor}}/>
                            {/*<ListItemText primary={photoCategory.categoryName} />*/}
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
        </>
    );
}
export default SelectPhotoCategory;