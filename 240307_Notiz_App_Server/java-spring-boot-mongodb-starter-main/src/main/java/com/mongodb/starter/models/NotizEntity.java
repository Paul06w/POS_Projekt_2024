package com.mongodb.starter.models;

import org.bson.types.ObjectId;

import java.util.Objects;

public class NotizEntity {
    private ObjectId id;
    private String title;
    private String text;
    private Boolean check;

    public NotizEntity() {
    }

    public NotizEntity(ObjectId id, String title, String text, Boolean checked) {
        this.id = id;
        this.title = title;
        this.text = text;
        this.check = checked;
    }

    public ObjectId getId() {
        return id;
    }

    public NotizEntity setId(ObjectId id) {
        this.id = id;
        return this;
    }

    public String getTitle() {
        return title;
    }

    public NotizEntity setTitle(String title) {
        this.title = title;
        return this;
    }

    public String getText() {
        return text;
    }

    public NotizEntity setText(String text) {
        this.text = text;
        return this;
    }

    public Boolean getCheck() {
        return check;
    }

    public NotizEntity setCheck(Boolean checked) {
        this.check = checked;
        return this;
    }

    @Override
    public String toString() {
        return "Notiz{" + "title='" + title + '\'' + ", text='" + text + ", checked='" + check + '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        NotizEntity notizEntity = (NotizEntity) o;
        return Objects.equals(title, notizEntity.title) && Objects.equals(text, notizEntity.text) && Objects.equals(check, notizEntity.check);
    }

    @Override
    public int hashCode() {
        return Objects.hash(title, text);
    }
}