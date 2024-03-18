package com.mongodb.starter.repositories;

import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.ClientSession;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.model.FindOneAndReplaceOptions;
import com.mongodb.client.model.ReplaceOneModel;
import com.mongodb.starter.models.NotizEntity;
import jakarta.annotation.PostConstruct;
import org.bson.BsonDocument;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;

import static com.mongodb.client.model.Aggregates.group;
import static com.mongodb.client.model.Filters.eq;
import static com.mongodb.client.model.Filters.in;
import static com.mongodb.client.model.ReturnDocument.AFTER;

@Repository
public class MongoDBNotizRepository implements NotizRepository {

    private static final TransactionOptions txnOptions = TransactionOptions.builder()
            .readPreference(ReadPreference.primary())
            .readConcern(ReadConcern.MAJORITY)
            .writeConcern(WriteConcern.MAJORITY)
            .build();
    private final MongoClient client;
    private MongoCollection<NotizEntity> notizCollection;

    public MongoDBNotizRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        notizCollection = client.getDatabase("app").getCollection("notizen", NotizEntity.class);
    }

    @Override
    public NotizEntity save(NotizEntity notizEntity) {
        notizEntity.setId(new ObjectId());
        notizCollection.insertOne(notizEntity);
        return notizEntity;
    }

    @Override
    public List<NotizEntity> saveAll(List<NotizEntity> notizEntities) {
        try (ClientSession clientSession = client.startSession()) {
            return clientSession.withTransaction(() -> {
                notizEntities.forEach(p -> p.setId(new ObjectId()));
                notizCollection.insertMany(clientSession, notizEntities);
                return notizEntities;
            }, txnOptions);
        }
    }

    @Override
    public List<NotizEntity> findAll() {
        return notizCollection.find().into(new ArrayList<>());
    }

    @Override
    public List<NotizEntity> findAll(List<String> ids) {
        return notizCollection.find(in("_id", mapToObjectIds(ids))).into(new ArrayList<>());
    }

    @Override
    public NotizEntity findOne(String id) {
        return notizCollection.find(eq("_id", new ObjectId(id))).first();
    }

    @Override
    public long count() {
        return notizCollection.countDocuments();
    }

    @Override
    public long delete(String id) {
        return notizCollection.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }

    @Override
    public long delete(List<String> ids) {
        try (ClientSession clientSession = client.startSession()) {
            return clientSession.withTransaction(
                    () -> notizCollection.deleteMany(clientSession, in("_id", mapToObjectIds(ids))).getDeletedCount(),
                    txnOptions);
        }
    }

    @Override
    public long deleteAll() {
        try (ClientSession clientSession = client.startSession()) {
            return clientSession.withTransaction(
                    () -> notizCollection.deleteMany(clientSession, new BsonDocument()).getDeletedCount(), txnOptions);
        }
    }

    @Override
    public NotizEntity update(NotizEntity notizEntity) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        return notizCollection.findOneAndReplace(eq("_id", notizEntity.getId()), notizEntity, options);
    }

    @Override
    public long update(List<NotizEntity> notizEntities) {
        List<ReplaceOneModel<NotizEntity>> writes = notizEntities.stream()
                .map(p -> new ReplaceOneModel<>(eq("_id", p.getId()),
                        p))
                .toList();
        try (ClientSession clientSession = client.startSession()) {
            return clientSession.withTransaction(
                    () -> notizCollection.bulkWrite(clientSession, writes).getModifiedCount(), txnOptions);
        }
    }

    private List<ObjectId> mapToObjectIds(List<String> ids) {
        return ids.stream().map(ObjectId::new).toList();
    }
}