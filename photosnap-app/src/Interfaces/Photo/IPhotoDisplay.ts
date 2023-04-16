interface PhotoDisplay{
    photoId: string,
    photo: string,
    description: string,
    numberOfFollowers: number,
    numberOfLikes: number,
    numberOfComments: number,
    authorUsername: string,
    authorProfilePhoto: string,
    categoryName: string,
    categoryColor: string
}
export default PhotoDisplay;