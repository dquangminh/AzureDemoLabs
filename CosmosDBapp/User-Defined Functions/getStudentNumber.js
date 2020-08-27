function getStudentNumber(student) {
    if (student.StudentNumber != undefined) {
        return student.StudentNumber;
    }

    // if neither are present, that is an error
    throw new Error("Document with id " + student.id + " does not contain StudentNumber ID in recognised format.");
}