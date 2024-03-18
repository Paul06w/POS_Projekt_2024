package com.mongodb.starter.models;

import org.bson.types.ObjectId;

import java.util.Objects;

public class NotizEntity {
    private ObjectId id;
    private String title;
    private String text;
    private Boolean checked;

    public NotizEntity() {
    }

    public NotizEntity(ObjectId id, String title, String text, Boolean checked) {
        this.id = id;
        this.title = title;
        this.text = text;
        this.checked = checked;
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

    public Boolean getChecked() {
        return checked;
    }

    public NotizEntity setChecked(Boolean checked) {
        this.checked = checked;
        return this;
    }

    @Override
    public String toString() {
        return "Notiz{" + "title='" + title + '\'' + ", text='" + text + ", checked='" + checked + '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        NotizEntity notizEntity = (NotizEntity) o;
        return Objects.equals(title, notizEntity.title) && Objects.equals(text, notizEntity.text) && Objects.equals(checked, notizEntity.checked);
    }

    @Override
    public int hashCode() {
        return Objects.hash(title, text);
    }
}