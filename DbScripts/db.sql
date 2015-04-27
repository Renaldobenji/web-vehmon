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
DROP DATABASE IF EXISTS `vehmon`;
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
DEFAULT CHARACTER SET = utf8
COMMENT = 'types of leave	';


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
  PRIMARY KEY (`auditEntryID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'store audit entries	';


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
  `authenticationTokenValue`  CHAR(36) NOT NULL,
  `userID` INT(11) NOT NULL,
  `ipAddress` VARCHAR(100) NULL DEFAULT NULL,
  `issueDate` DATETIME NOT NULL,
  `lastActivityDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`authenticationTokenValue`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'token base authentication	';
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
  PRIMARY KEY (`timeTrackingID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'time tracking table	';

-- -----------------------------------------------------
-- Table `vehmon`.`Route`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`route` (
  `routeID` INT(11) NOT NULL AUTO_INCREMENT,
  `timeTrackingID` INT(11) NOT NULL,
  `startTime` DATETIME NOT NULL,
  `endTime` DATETIME NULL DEFAULT NULL,
  `clockInLat` DECIMAL(9,6) NULL DEFAULT NULL,
  `clockInLng` DECIMAL(9,6) NULL DEFAULT NULL,
  PRIMARY KEY (`routeID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'route table	';

-- -----------------------------------------------------
-- Table `vehmon`.`Coord`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`coord` (
  `coordID` INT(11) NOT NULL AUTO_INCREMENT,
  `routeID` INT(11) NOT NULL,
  `time` DATETIME NOT NULL,
  `lat` DECIMAL(9,6) NULL DEFAULT NULL,
  `lng` DECIMAL(9,6) NULL DEFAULT NULL,
  PRIMARY KEY (`coordID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'Coordinate table	';


-- -----------------------------------------------------
-- Table `vehmon`.`userabsence`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`userabsence` (
  `userAbsenseID` INT(11) NOT NULL AUTO_INCREMENT,
  `userId` INT(11) NOT NULL,
  `absenseTypeID` INT(11) NOT NULL,
  `fromDate` DATETIME NOT NULL,
  `toDate` DATETIME NOT NULL,
  PRIMARY KEY (`userAbsenseID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'Stores leave taken by the user';


-- -----------------------------------------------------
-- Table `vehmon`.`userrolemapping`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`userrolemapping` (
  `userRoleMappingID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `roleID` INT(11) NOT NULL,
  `fromDate` DATETIME NOT NULL,
  `toDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`userRoleMappingID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'user role mapping		';

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
  `deviceID` VARCHAR(100) NULL DEFAULT NULL,
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
  PRIMARY KEY (`userID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'User		';


-- -----------------------------------------------------
-- Table `vehmon`.`messages`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`messages` (
  `messageID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `conversationID` INT(11) NOT NULL,
  `messageText` NVARCHAR(512) NOT NULL,
  `dateSent` DATETIME NOT NULL,
  PRIMARY KEY (`messageID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'mesages';

-- -----------------------------------------------------
-- Table `vehmon`.`conversation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`conversation` (
  `conversationID` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(64) NOT NULL,
  `dateCreated` DATETIME NOT NULL,
  PRIMARY KEY (`conversationID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'conversation';

-- -----------------------------------------------------
-- Table `vehmon`.`userMessageReceipt`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`userMessageReceipt` (
  `userMessageReceiptID` INT(11) NOT NULL AUTO_INCREMENT,
  `messageID` INT(11) NOT NULL,
  `hasReceived` BIT NULL,
  `userID` INT(11) NOT NULL,
  PRIMARY KEY (`userMessageReceiptID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'userMessageReceipt';
-- -----------------------------------------------------
-- Table `vehmon`.`messages`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`messages` (
  `messageID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `conversationID` INT(11) NOT NULL,
  `messageText` NVARCHAR(512) NOT NULL,
  PRIMARY KEY (`messageID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'mesages';

-- -----------------------------------------------------
-- Table `vehmon`.`absencetype`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`absencetype` (
  `absenceTypeID` INT(11) NOT NULL AUTO_INCREMENT,
  `absenceTypeCode` VARCHAR(45) NOT NULL,
  `absenceTypeDescription` VARCHAR(500) NOT NULL,
  PRIMARY KEY (`absenceTypeID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'types of leave	';

-- -----------------------------------------------------
-- Table `vehmon`.`userConversation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`userConversation` (
  `userConversationID` INT(11) NOT NULL AUTO_INCREMENT,
  `conversationID` INT(11) NOT NULL,
  `userID` INT(11) NOT NULL,
  `isHidden` BIT NOT NULL,
  PRIMARY KEY (`userConversationID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'userConversation';

-- -----------------------------------------------------
-- Table `vehmon`.`userConversation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vehmon`.`company` (
  `companyID` INT(11) NOT NULL AUTO_INCREMENT,
  `companyName` NVARCHAR(512) NOT NULL,
  `isActive` BIT NOT NULL,
  `dateAdded` DATETIME NOT NULL,
  `userCount` INT(11) NOT NULL,
  PRIMARY KEY (`companyID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'companyID';

ALTER TABLE `vehmon`.`users`
ADD CONSTRAINT FOREIGN KEY (companyID) REFERENCES vehmon.company(companyID);

-- Links user table and uthenticatin token
ALTER TABLE `vehmon`.`authenticationtoken`
ADD CONSTRAINT FOREIGN KEY (userID) REFERENCES vehmon.users(userID);

ALTER TABLE `vehmon`.`auditentry`
ADD CONSTRAINT FOREIGN KEY (userID) REFERENCES vehmon.users(userID);

ALTER TABLE `vehmon`.`userabsence`
ADD CONSTRAINT FOREIGN KEY (userID) REFERENCES vehmon.users(userID);

ALTER TABLE `vehmon`.`timetracking`
ADD CONSTRAINT FOREIGN KEY (userID) REFERENCES vehmon.users(userID);

ALTER TABLE `vehmon`.`userabsence`
ADD CONSTRAINT FOREIGN KEY (absenseTypeID) REFERENCES vehmon.absencetype(absenceTypeID);

ALTER TABLE `vehmon`.`route`
ADD CONSTRAINT FOREIGN KEY (timeTrackingID) REFERENCES vehmon.timetracking(timeTrackingID);

ALTER TABLE `vehmon`.`coord`
ADD CONSTRAINT FOREIGN KEY (routeID) REFERENCES vehmon.route(routeID);

ALTER TABLE `vehmon`.`userrolemapping`
ADD CONSTRAINT FOREIGN KEY (userID) REFERENCES vehmon.users(userID);

ALTER TABLE `vehmon`.`userrolemapping`
ADD CONSTRAINT FOREIGN KEY (roleID) REFERENCES vehmon.roles(roleID);

ALTER TABLE `vehmon`.`messages`
ADD CONSTRAINT FOREIGN KEY (userID) REFERENCES vehmon.users(userID);

ALTER TABLE `vehmon`.`messages`
ADD CONSTRAINT FOREIGN KEY (conversationID) REFERENCES vehmon.conversation(conversationID);

ALTER TABLE `vehmon`.`userMessageReceipt`
ADD CONSTRAINT FOREIGN KEY (userID) REFERENCES vehmon.users(userID);

ALTER TABLE `vehmon`.`userMessageReceipt`
ADD CONSTRAINT FOREIGN KEY (messageID) REFERENCES vehmon.messages(messageID);

ALTER TABLE `vehmon`.`userConversation`
ADD CONSTRAINT FOREIGN KEY (conversationID) REFERENCES vehmon.conversation(conversationID);

ALTER TABLE `vehmon`.`userConversation`
ADD CONSTRAINT FOREIGN KEY (userID) REFERENCES vehmon.users(userID);


INSERT INTO `vehmon`.`absencetype` (`absenceTypeCode`,`absenceTypeDescription`)
VALUES ('Annual','Annual leave');

INSERT INTO `vehmon`.`absencetype` (`absenceTypeCode`,`absenceTypeDescription`)
VALUES ('Sick','Sick leave');

INSERT INTO `vehmon`.`absencetype` (`absenceTypeCode`,`absenceTypeDescription`)
VALUES ('Canceled','Canceled leave');


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
