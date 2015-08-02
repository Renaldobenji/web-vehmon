-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema vehmon
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema vehmon
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `vehmon` DEFAULT CHARACTER SET utf8 ;
USE `vehmon` ;

-- -----------------------------------------------------
-- Table `vehmon`.`absencetype`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`absencetype` (
  `absenceTypeID` INT(11) NOT NULL AUTO_INCREMENT,
  `absenceTypeCode` VARCHAR(45) NOT NULL,
  `absenceTypeDescription` VARCHAR(500) NOT NULL,
  PRIMARY KEY (`absenceTypeID`))
ENGINE = InnoDB
AUTO_INCREMENT = 8
DEFAULT CHARACTER SET = utf8
COMMENT = 'types of leave	';


-- -----------------------------------------------------
-- Table `vehmon`.`company`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`company` (
  `companyID` INT(11) NOT NULL AUTO_INCREMENT,
  `companyName` VARCHAR(512) NOT NULL,
  `isActive` BIT(1) NOT NULL,
  `dateAdded` DATETIME NOT NULL,
  `userCount` INT(11) NOT NULL,
  PRIMARY KEY (`companyID`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = utf8
COMMENT = 'companyID';


-- -----------------------------------------------------
-- Table `vehmon`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`users` (
  `userID` INT(11) NOT NULL AUTO_INCREMENT,
  `companyID` INT(11) NOT NULL,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `passwordSalt` VARCHAR(45) NOT NULL,
  `title` VARCHAR(45) NOT NULL,
  `firstname` VARCHAR(100) NOT NULL,
  `surname` VARCHAR(100) NOT NULL,
  `employerNumber` VARCHAR(45) NOT NULL,
  `identificationNumber` VARCHAR(45) NOT NULL,
  `deviceID` VARCHAR(500) NULL DEFAULT NULL,
  `emailAddress` VARCHAR(100) NULL DEFAULT NULL,
  `cellNumber` VARCHAR(45) NULL DEFAULT NULL,
  `isApproved` BIT(1) NOT NULL,
  `isLockedOut` VARCHAR(45) NOT NULL,
  `lastActivityDate` DATETIME NULL DEFAULT NULL,
  `lastPasswordChange` DATETIME NULL DEFAULT NULL,
  `lastLoginDate` DATETIME NULL DEFAULT NULL,
  `failedPasswordAttemptCount` INT(11) NULL DEFAULT NULL,
  `created` DATETIME NOT NULL,
  `updated` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`userID`),
  CONSTRAINT `users_ibfk_1`
    FOREIGN KEY (`companyID`)
    REFERENCES `vehmon`.`company` (`companyID`))
ENGINE = InnoDB
AUTO_INCREMENT = 12
DEFAULT CHARACTER SET = utf8
COMMENT = 'User		';

CREATE INDEX `companyID` ON `vehmon`.`users` (`companyID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`auditentry`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`auditentry` (
  `auditEntryID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `auditEntryOwnerType` VARCHAR(45) NOT NULL,
  `auditEntryOwnerTypeID` VARCHAR(45) NOT NULL,
  `auditEntrySummary` VARCHAR(250) NOT NULL,
  `auditEntryDetail` VARCHAR(1000) NOT NULL,
  `auditEntryComment` VARCHAR(250) NULL DEFAULT NULL,
  `auditEntryTime` DATETIME NOT NULL,
  PRIMARY KEY (`auditEntryID`),
  CONSTRAINT `auditentry_ibfk_1`
    FOREIGN KEY (`userID`)
    REFERENCES `vehmon`.`users` (`userID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'store audit entries	';

CREATE INDEX `userID` ON `vehmon`.`auditentry` (`userID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`auditentrytype`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`auditentrytype` (
  `auditEntryTypeID` INT(11) NOT NULL AUTO_INCREMENT,
  `auditEntryOwnerType` VARCHAR(45) NOT NULL,
  `auditEntryName` VARCHAR(45) NOT NULL,
  `auditEntryDescription` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`auditEntryTypeID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'Audit Entry Types		';


-- -----------------------------------------------------
-- Table `vehmon`.`authenticationtoken`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`authenticationtoken` (
  `authenticationTokenValue` CHAR(36) NOT NULL,
  `userID` INT(11) NOT NULL,
  `ipAddress` VARCHAR(100) NULL DEFAULT NULL,
  `issueDate` DATETIME NOT NULL,
  `lastActivityDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`authenticationTokenValue`),
  CONSTRAINT `authenticationtoken_ibfk_1`
    FOREIGN KEY (`userID`)
    REFERENCES `vehmon`.`users` (`userID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'token base authentication	';

CREATE INDEX `userID` ON `vehmon`.`authenticationtoken` (`userID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`conversation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`conversation` (
  `conversationID` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(64) NOT NULL,
  `dateCreated` DATETIME NOT NULL,
  PRIMARY KEY (`conversationID`))
ENGINE = InnoDB
AUTO_INCREMENT = 177
DEFAULT CHARACTER SET = utf8
COMMENT = 'conversation';


-- -----------------------------------------------------
-- Table `vehmon`.`timetracking`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`timetracking` (
  `timeTrackingID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `clockInTime` DATETIME NOT NULL,
  `clockOutTime` DATETIME NULL DEFAULT NULL,
  `clockInLat` DECIMAL(9,6) NULL DEFAULT NULL,
  `clockInLng` DECIMAL(9,6) NULL DEFAULT NULL,
  `clockOutLat` DECIMAL(9,6) NULL DEFAULT NULL,
  `clockOutLng` DECIMAL(9,6) NULL DEFAULT NULL,
  PRIMARY KEY (`timeTrackingID`),
  CONSTRAINT `timetracking_ibfk_1`
    FOREIGN KEY (`userID`)
    REFERENCES `vehmon`.`users` (`userID`))
ENGINE = InnoDB
AUTO_INCREMENT = 87
DEFAULT CHARACTER SET = utf8
COMMENT = 'time tracking table	';

CREATE INDEX `userID` ON `vehmon`.`timetracking` (`userID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`route`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`route` (
  `routeID` INT(11) NOT NULL AUTO_INCREMENT,
  `timeTrackingID` INT(11) NOT NULL,
  `startTime` DATETIME NOT NULL,
  `endTime` DATETIME NULL DEFAULT NULL,
  `clockInLat` DECIMAL(9,6) NULL DEFAULT NULL,
  `clockInLng` DECIMAL(9,6) NULL DEFAULT NULL,
  PRIMARY KEY (`routeID`),
  CONSTRAINT `route_ibfk_1`
    FOREIGN KEY (`timeTrackingID`)
    REFERENCES `vehmon`.`timetracking` (`timeTrackingID`))
ENGINE = InnoDB
AUTO_INCREMENT = 56
DEFAULT CHARACTER SET = utf8
COMMENT = 'route table	';

CREATE INDEX `timeTrackingID` ON `vehmon`.`route` (`timeTrackingID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`coord`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`coord` (
  `coordID` INT(11) NOT NULL AUTO_INCREMENT,
  `routeID` INT(11) NOT NULL,
  `time` DATETIME NOT NULL,
  `lat` DECIMAL(9,6) NULL DEFAULT NULL,
  `lng` DECIMAL(9,6) NULL DEFAULT NULL,
  PRIMARY KEY (`coordID`),
  CONSTRAINT `coord_ibfk_1`
    FOREIGN KEY (`routeID`)
    REFERENCES `vehmon`.`route` (`routeID`))
ENGINE = InnoDB
AUTO_INCREMENT = 7801
DEFAULT CHARACTER SET = utf8
COMMENT = 'Coordinate table	';

CREATE INDEX `routeID` ON `vehmon`.`coord` (`routeID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`devicecoordinates`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`devicecoordinates` (
  `deviceCoordinatesID` INT(11) NOT NULL AUTO_INCREMENT,
  `deviceID` VARCHAR(100) NOT NULL,
  `deviceLat` DECIMAL(9,6) NOT NULL,
  `deviceLng` DECIMAL(9,6) NOT NULL,
  `dateTime` DATETIME NOT NULL,
  PRIMARY KEY (`deviceCoordinatesID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'This will store the coordinates of a specific device';


-- -----------------------------------------------------
-- Table `vehmon`.`groupmembership`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`groupmembership` (
  `groupMembershipID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `groupCode` VARCHAR(50) NOT NULL,
  `fromDate` DATETIME NOT NULL,
  `toDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`groupMembershipID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'group membership';


-- -----------------------------------------------------
-- Table `vehmon`.`grouprolemapping`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`grouprolemapping` (
  `groupRoleMappingID` INT(11) NOT NULL AUTO_INCREMENT,
  `groupCode` VARCHAR(45) NOT NULL,
  `roleCode` VARCHAR(45) NOT NULL,
  `fromDate` DATETIME NOT NULL,
  `toDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`groupRoleMappingID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'Maps group to roles		';


-- -----------------------------------------------------
-- Table `vehmon`.`groups`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`groups` (
  `groupID` INT(11) NOT NULL AUTO_INCREMENT,
  `groupCode` VARCHAR(45) NOT NULL,
  `groupName` VARCHAR(100) NOT NULL,
  `groupDescription` VARCHAR(100) NULL DEFAULT NULL,
  `groupStatus` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`groupID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'Groups for the application		';


-- -----------------------------------------------------
-- Table `vehmon`.`messages`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`messages` (
  `messageID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `conversationID` INT(11) NOT NULL,
  `messageText` VARCHAR(512) NOT NULL,
  `dateSent` DATETIME NOT NULL,
  PRIMARY KEY (`messageID`),
  CONSTRAINT `messages_ibfk_1`
    FOREIGN KEY (`userID`)
    REFERENCES `vehmon`.`users` (`userID`),
  CONSTRAINT `messages_ibfk_2`
    FOREIGN KEY (`conversationID`)
    REFERENCES `vehmon`.`conversation` (`conversationID`))
ENGINE = InnoDB
AUTO_INCREMENT = 836
DEFAULT CHARACTER SET = utf8
COMMENT = 'mesages';

CREATE INDEX `userID` ON `vehmon`.`messages` (`userID` ASC);

CREATE INDEX `conversationID` ON `vehmon`.`messages` (`conversationID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`roles`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`roles` (
  `roleID` INT(11) NOT NULL AUTO_INCREMENT,
  `roleCode` VARCHAR(45) NOT NULL,
  `roleName` VARCHAR(100) NOT NULL,
  `roleDescription` VARCHAR(200) NULL DEFAULT NULL,
  `roleStatus` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`roleID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'Roles Table used for Authorization	';


-- -----------------------------------------------------
-- Table `vehmon`.`userabsence`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`userabsence` (
  `userAbsenseID` INT(11) NOT NULL AUTO_INCREMENT,
  `userId` INT(11) NOT NULL,
  `absenseTypeID` INT(11) NOT NULL,
  `fromDate` DATETIME NOT NULL,
  `toDate` DATETIME NOT NULL,
  `approved` BIT(1) NULL DEFAULT NULL,
  PRIMARY KEY (`userAbsenseID`),
  CONSTRAINT `userabsence_ibfk_1`
    FOREIGN KEY (`userId`)
    REFERENCES `vehmon`.`users` (`userID`),
  CONSTRAINT `userabsence_ibfk_2`
    FOREIGN KEY (`absenseTypeID`)
    REFERENCES `vehmon`.`absencetype` (`absenceTypeID`))
ENGINE = InnoDB
AUTO_INCREMENT = 41
DEFAULT CHARACTER SET = utf8
COMMENT = 'Stores leave taken by the user';

CREATE INDEX `userId` ON `vehmon`.`userabsence` (`userId` ASC);

CREATE INDEX `absenseTypeID` ON `vehmon`.`userabsence` (`absenseTypeID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`userabsencebalance`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`userabsencebalance` (
  `iduserabsencebalanceID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `amount` DECIMAL(5,0) NOT NULL,
  `datestamp` DATETIME NOT NULL,
  PRIMARY KEY (`iduserabsencebalanceID`),
  CONSTRAINT `userID`
    FOREIGN KEY (`userID`)
    REFERENCES `vehmon`.`users` (`userID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 19
DEFAULT CHARACTER SET = utf8
COMMENT = 'running  balance of the users leave';

CREATE INDEX `userID_idx` ON `vehmon`.`userabsencebalance` (`userID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`userconversation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`userconversation` (
  `userConversationID` INT(11) NOT NULL AUTO_INCREMENT,
  `conversationID` INT(11) NOT NULL,
  `userID` INT(11) NOT NULL,
  `isHidden` BIT(1) NOT NULL,
  PRIMARY KEY (`userConversationID`),
  CONSTRAINT `userconversation_ibfk_1`
    FOREIGN KEY (`conversationID`)
    REFERENCES `vehmon`.`conversation` (`conversationID`),
  CONSTRAINT `userconversation_ibfk_2`
    FOREIGN KEY (`userID`)
    REFERENCES `vehmon`.`users` (`userID`))
ENGINE = InnoDB
AUTO_INCREMENT = 352
DEFAULT CHARACTER SET = utf8
COMMENT = 'userConversation';

CREATE INDEX `conversationID` ON `vehmon`.`userconversation` (`conversationID` ASC);

CREATE INDEX `userID` ON `vehmon`.`userconversation` (`userID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`usermessagereceipt`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`usermessagereceipt` (
  `userMessageReceiptID` INT(11) NOT NULL AUTO_INCREMENT,
  `messageID` INT(11) NOT NULL,
  `hasReceived` BIT(1) NULL DEFAULT NULL,
  `userID` INT(11) NOT NULL,
  PRIMARY KEY (`userMessageReceiptID`),
  CONSTRAINT `usermessagereceipt_ibfk_1`
    FOREIGN KEY (`userID`)
    REFERENCES `vehmon`.`users` (`userID`),
  CONSTRAINT `usermessagereceipt_ibfk_2`
    FOREIGN KEY (`messageID`)
    REFERENCES `vehmon`.`messages` (`messageID`))
ENGINE = InnoDB
AUTO_INCREMENT = 1676
DEFAULT CHARACTER SET = utf8
COMMENT = 'userMessageReceipt';

CREATE INDEX `userID` ON `vehmon`.`usermessagereceipt` (`userID` ASC);

CREATE INDEX `messageID` ON `vehmon`.`usermessagereceipt` (`messageID` ASC);


-- -----------------------------------------------------
-- Table `vehmon`.`userrolemapping`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`userrolemapping` (
  `userRoleMappingID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `roleID` INT(11) NOT NULL,
  `fromDate` DATETIME NOT NULL,
  `toDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`userRoleMappingID`),
  CONSTRAINT `userrolemapping_ibfk_1`
    FOREIGN KEY (`userID`)
    REFERENCES `vehmon`.`users` (`userID`),
  CONSTRAINT `userrolemapping_ibfk_2`
    FOREIGN KEY (`roleID`)
    REFERENCES `vehmon`.`roles` (`roleID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'user role mapping		';

CREATE INDEX `userID` ON `vehmon`.`userrolemapping` (`userID` ASC);

CREATE INDEX `roleID` ON `vehmon`.`userrolemapping` (`roleID` ASC);

USE `vehmon` ;

-- -----------------------------------------------------
-- procedure sps_CalculateTotalHoursWorked
-- -----------------------------------------------------

DELIMITER $$
USE `vehmon`$$
CREATE DEFINER=`VehmonAdmin`@`%` PROCEDURE `sps_CalculateTotalHoursWorked`(
									IN UserIDList varchar(5000),
                                    IN StartDate varchar(100),
                                    IN EndDate varchar(100)
								  )
BEGIN

	SELECT u.firstname, u.surname, SUM(TIMESTAMPDIFF(HOUR, ClockInTime , ClockOutTime)) AS TotalHoursWorked
    FROM timetracking t
    INNER JOIN users u ON t.UserID = u.UserID
    WHERE FIND_IN_SET(u.UserID,UserIDList)
    AND ClockInTime >= StartDate AND ClockOutTime <= EndDate
    AND ClockOutTime IS NOT NULL
    GROUP BY  u.firstname, u.surname;

END$$

DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
