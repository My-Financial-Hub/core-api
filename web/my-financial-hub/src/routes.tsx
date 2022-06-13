import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import App from './pages/App';
import NotFoundPage from './pages/not-found';
import AccountsPage from './pages/accounts';

export default function AppRouter() {
  return (
    <Router>
      <Routes>
        <Route path='/' element={<App />} />
        <Route path='/accounts' element={<AccountsPage />} />
        <Route path='*' element={<NotFoundPage />} />
      </Routes>
    </Router>
  );
}