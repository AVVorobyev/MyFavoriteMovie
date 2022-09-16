import React from "react";
import { useParams } from "react-router-dom";
import { Actor } from './Actor';

export const ActorByIdWrapper = () => {
    const { actorId } = useParams();
    return <Actor actorId={actorId} />
}