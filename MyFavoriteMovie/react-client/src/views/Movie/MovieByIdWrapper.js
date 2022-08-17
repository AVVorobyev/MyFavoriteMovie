import { React } from 'react';
import { useParams } from 'react-router-dom';
import { Movie } from './Movie';

export const MovieByIdWrapper = () => {
    const { movieId } = useParams();
    return <Movie movieId={movieId} />
}