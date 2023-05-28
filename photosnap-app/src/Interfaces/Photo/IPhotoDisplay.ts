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
    categoryColor: string,
    photoDeleted(photoId: string): void | undefined
}
export default PhotoDisplay;