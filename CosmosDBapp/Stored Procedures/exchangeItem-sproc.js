function exchangeItemsSproc (studentId1, studentId2) {
    var collection = getContext().getCollection();
    var response = getContext().getResponse();
    var student1Document, student2Document;

    var filterQuery =
    {
        'query' : 'SELECT * FROM root s WHERE s.id = @studentId1',
        'parameters' : [{'name': '@studentId1', 'value': studentId1}]
    };
    // Query documents and take 1st item.
    var isAccepted = collection.queryDocuments(
        collection.getSelfLink(),
        filterQuery,
        function (err, feed, options) {
            if (err) throw err;

            if (!feed || !feed.length) {
                response.setBody('no docs found');
            }
            else {
                student1Document = feed[0];
                var filterQuery2 =
                {
                    'query' : 'SELECT * FROM StudentCourseGrades s WHERE s.id = @studentId2',
                    'parameters' : [{'name': '@studentId2', 'value': studentId2}]
                };
                var isAccepted2 = collection.queryDocuments(
                    collection.getSelfLink(),
                    filterQuery2,
                    function (err, feed, options) {
                        if (err) throw err;
                
                        if (!feed || !feed.length) {
                            response.setBody('no docs found');
                        }
                        else {
                            student2Document = feed[0];
                            swapCourseGrades(student1Document, student2Document);
                            response.setBody('OK done!');
                        }
                    
                    }
                );
                
                if (!isAccepted2) throw new Error('The query was not accepted by the server.');
            }
        }
    );

    if (!isAccepted) throw new Error('The query was not accepted by the server.');

    function swapCourseGrades (student1, student2) {
        var tempCourseGrades = student1.CourseGrades;
        student1.CourseGrades = student2.CourseGrades;
        student2.CourseGrades = tempCourseGrades;

        var isAccepted = collection.replaceDocument(
            student1Document._self,
            student1Document,
            function (err, docReplaced) {
                if (err) throw err;

                var isAccepted2 = collection.replaceDocument(
                    student2Document._self,
                    student2Document,
                    function (err, docReplaced) {
                        if (err) throw err;
                    }
                );

                if (!isAccepted2) throw "The query was not accepted by the server.";
            }
        );

        if (!isAccepted) throw "The query was not accepted by the server.";
    }
};