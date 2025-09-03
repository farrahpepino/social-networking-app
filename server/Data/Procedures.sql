USE LoopDb;

DELIMITER //

CREATE PROCEDURE AddInterestToUser(
    IN p_userId1 VARCHAR(36),
    IN p_interestId VARCHAR(36)
)
BEGIN
    UPDATE users
    SET Interests = JSON_ARRAY_APPEND(
        COALESCE(Interests, JSON_ARRAY()), '$', p_interestId
    )
    WHERE Id = p_userId1
      AND JSON_CONTAINS(COALESCE(Interests, JSON_ARRAY()), JSON_QUOTE(p_interestId), '$') = 0
    LIMIT 1;
END//

DELIMITER ;


DELIMITER //

CREATE PROCEDURE CreateInterest(
    IN p_userId1 VARCHAR(36),
    IN p_userId2 VARCHAR(36),
    IN p_interestId VARCHAR(36),
    IN p_createdAt TIMESTAMP
)
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM interests
        WHERE UserId1 = p_userId1
          AND UserId2 = p_userId2
        LIMIT 1
    ) THEN
        INSERT INTO interests (Id, UserId1, UserId2, CreatedAt)
        VALUES (p_interestId, p_userId1, p_userId2, p_createdAt);
        
        CALL AddInterestToUser(p_userId1, p_interestId);
    END IF;
END//

DELIMITER ;
