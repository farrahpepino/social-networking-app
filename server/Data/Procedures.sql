
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
    END IF;
END//

DELIMITER ;
