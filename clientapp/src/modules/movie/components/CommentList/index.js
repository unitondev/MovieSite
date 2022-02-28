import React from 'react'
import PropTypes from 'prop-types'
import withStyles from '@mui/styles/withStyles';
import { Avatar, Button, Card, CardActions, CardContent, CardHeader, Typography } from '@mui/material'
import * as Yup from 'yup'
import { TextField } from 'formik-mui'

import Comment from '../Comment'
import styles from './styles'
import { Field, Formik, Form } from 'formik'

const writtenCommentSchema = Yup.object().shape({
  WrittenCommentText: Yup.string()
    .trim()
    .max(1024, 'Maximum length is 1024 characters'),
})

const CommentList = ({
  classes,
  currentUserAvatar,
  handleCommentSet,
  comments,
  currentUser,
  handleDeleteCommentClick,
  dialogProps,
}) => (
  <div className={classes.commentsBlock}>
    <Typography variant='h3'>Comments</Typography>
    <Formik
      initialValues={{ WrittenCommentText: ''}}
      enableReinitialize
      validationSchema={writtenCommentSchema}
      onSubmit={(values, formikBag) => {
        handleCommentSet(values)
        formikBag.resetForm()
      }}
    >
    {({ dirty, isValid }) => (
      <div className={classes.commentBlock}>
        <Card>
          <Form className={classes.form} autoComplete='off'>
            <CardHeader
              avatar={
                <Avatar src={currentUserAvatar} />
              }
              title={`As ${currentUser.userName}`}
              titleTypographyProps={{ fontSize: 18 }}
            />
            <CardContent>
              <Field
                multiline
                placeholder='Write your comment here'
                variant='outlined'
                name='WrittenCommentText'
                component={TextField}
                className={classes.writingCommentTextArea}
              />
            </CardContent>
            <CardActions>
              <Button
                disabled={!(isValid && dirty)}
                color='primary'
                size='large'
                fullWidth
                type='submit'
              >
                Send
              </Button>
            </CardActions>
          </Form>
        </Card>
      </div>
    )}
    </Formik>
    {
      comments.length > 0
        ? (
            comments.slice().reverse().map((comment) => (
              <Comment
                key={comment.commentId}
                comment={comment}
                currentUserUserName={currentUser.userName}
                handleDeleteCommentClick={handleDeleteCommentClick}
                dialogProps={dialogProps}
              />
            ))
        )
        : (
          <div className={classes.emptyCommentsBlock} />
        )
    }
  </div>
)

CommentList.propTypes = {
  classes: PropTypes.object.isRequired,
  currentUserAvatar: PropTypes.string.isRequired,
  handleCommentSet: PropTypes.func.isRequired,
  comments: PropTypes.array.isRequired,
  currentUser: PropTypes.object.isRequired,
  handleDeleteCommentClick: PropTypes.func.isRequired,
  dialogProps: PropTypes.object.isRequired,
}

export default withStyles(styles)(CommentList)
