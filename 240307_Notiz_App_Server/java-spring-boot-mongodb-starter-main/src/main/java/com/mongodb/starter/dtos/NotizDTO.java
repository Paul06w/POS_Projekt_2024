package com.mongodb.starter.dtos;

import com.mongodb.starter.models.NotizEntity;
import org.bson.types.ObjectId;

public record NotizDTO(String id, String title, String text, Boolean check) {

    public NotizDTO(NotizEntity n) {
        this(n.getId() == null ? new ObjectId().toHexString() : n.getId().toHexString(),n.getTitle(), n.getText(), n.getCheck());
    }

    public NotizEntity toNotizEntity() {
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        return new NotizEntity(_id, title, text, check);
    }
}