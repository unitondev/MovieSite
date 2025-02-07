import { Typography, Box, Container, Link } from '@mui/material'
import Copyright from '../Copyright'
import withStyles from '@mui/styles/withStyles'

import styles from './styles'

const StickyFooter = ({ classes, hideFooter, children }) => (
  <Box className={!hideFooter ? classes.rootBox : null}>
    {children}
    <Box hidden={hideFooter} className={classes.footerBlock} component='footer'>
      <Container maxWidth='sm'>
        <Typography variant='body1' align='center'>
          View source code on{' '}
          <Link color='inherit' href='https://github.com/unitondev/Kvikmynd'>
            Github
          </Link>
        </Typography>
        <Copyright />
      </Container>
    </Box>
  </Box>
)

export default withStyles(styles)(StickyFooter)
